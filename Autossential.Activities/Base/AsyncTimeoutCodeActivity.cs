using Autossential.Activities.Extensions;
using System.Activities;

namespace Autossential.Activities.Base
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

            var task = RunAsync(context, cts.Token).WithTimeout(timeout, ContinueOnError.Get(context), cts.Token);

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

        protected abstract Task<T> RunAsync(AsyncCodeActivityContext context, CancellationToken token);

        protected override T EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var task = (Task<T>)result;
            return task.GetAwaiter().GetResult(); // avoids AggregateException
        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            var cts = context.UserState as CancellationTokenSource;
            cts?.Cancel();
        }
    }

    public abstract class AsyncTimeoutCodeActivity : AsyncCodeActivity
    {
        public InArgument<double> TimeoutSeconds { get; set; } = 30;
        public virtual InArgument<bool> ContinueOnError { get; set; } = false;

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            var timeout = TimeSpan.FromSeconds(TimeoutSeconds.Get(context));

            var cts = new CancellationTokenSource();
            context.UserState = cts;

            var task = RunAsync(context, cts.Token).WithTimeout(timeout, ContinueOnError.Get(context), cts.Token);

            var tcs = new TaskCompletionSource<Action<AsyncCodeActivityContext>>(state);
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

        protected abstract Task<Action<AsyncCodeActivityContext>> RunAsync(AsyncCodeActivityContext context, CancellationToken token);

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var task = (Task<Action<AsyncCodeActivityContext>>)result;
            task.GetAwaiter().GetResult(); // avoids AggregateException
        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            var cts = context.UserState as CancellationTokenSource;
            cts?.Cancel();
        }
    }
}
