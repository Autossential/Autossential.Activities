using System.Activities;

namespace Autossential.Activities.Base
{
    public abstract class AsynchronousCodeActivity : AsyncCodeActivity
    {
        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            var cts = new CancellationTokenSource();
            context.UserState = cts;

            var tcs = new TaskCompletionSource<Action<AsyncCodeActivityContext>>(state);

            RunAsync(context, cts.Token).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    tcs.TrySetException(t.Exception.Flatten());
                else if (t.IsCanceled || cts.IsCancellationRequested)
                    tcs.TrySetCanceled();
                else
                    tcs.TrySetResult(t.Result);

                callback?.Invoke(tcs.Task);
                cts.Dispose();
            }, TaskScheduler.Default);

            return tcs.Task;
        }

        protected abstract Task<Action<AsyncCodeActivityContext>> RunAsync(AsyncCodeActivityContext context, CancellationToken token);

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            var task = (Task<Action<AsyncCodeActivityContext>>)result;
            task.Result?.Invoke(context);
        }

        protected override void Cancel(AsyncCodeActivityContext context)
        {
            var cts = context.UserState as CancellationTokenSource;
            cts?.Cancel();
        }
    }
}
