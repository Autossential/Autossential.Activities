using System.Activities;

namespace Autossential.Activities
{
    public abstract class AsyncTimeoutCodeActivity<T> : AsyncCodeActivity<T>
    {
        public InArgument<double> TimeoutSeconds { get; set; } = 30;
        public virtual InArgument<bool> ContinueOnError { get; set; } = false;

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            var timeout = TimeSpan.FromSeconds(TimeoutSeconds.Get(context));

            var cts = new CancellationTokenSource();
            context.UserState = cts;

            var task = ExecuteWithTimeoutAsync(context, timeout, cts.Token);

            var tcs = new TaskCompletionSource<T>(state);
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception?.InnerException ?? t.Exception);
                else if (t.IsCanceled || cts.IsCancellationRequested)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                callback?.Invoke(tcs.Task);
                cts.Dispose();
            });

            return tcs.Task;
        }

        private async Task<T> ExecuteWithTimeoutAsync(AsyncCodeActivityContext context, TimeSpan timeout, CancellationToken token)
        {
            var continueOnError = ContinueOnError.Get(context);

            try
            {
                var runTask = RunAsync(context, token);
                var timeoutTask = Task.Delay(timeout, token);

                var completedTask = await Task.WhenAny(runTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    throw new TimeoutException("The operation has timed-out.");
                }

                return await runTask;
            }
            catch
            {
                if (continueOnError)
                    return default;

                throw;
            }
        }

        protected abstract Task<T> RunAsync(AsyncCodeActivityContext context, CancellationToken token);

        protected override T EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var task = (Task<T>)result;
            return task.GetAwaiter().GetResult(); // Evita AggregateException
        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            var cts = context.UserState as CancellationTokenSource;
            cts?.Cancel();
        }
    }
}
