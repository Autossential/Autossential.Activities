using Autossential.Activities.Properties;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class WaitFile : ContinuableAsyncTaskCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        public InArgument<int> Timeout { get; set; } = 30000;

        public InArgument<bool> WaitForExist { get; set; }

        public InArgument<int> Interval { get; set; } = 500;

        public OutArgument<FileInfo> Result { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var path = FilePath.Get(context);
            var time = Timeout.Get(context);
            var waitForExist = WaitForExist.Get(context);
            var interval = Math.Max(Interval.Get(context), 50);

            await ExecuteWithTimeoutAsync(context, token, ExecuteMainAsync(path, waitForExist, interval, token), time, defaultHandler =>
            {
                throw new TimeoutException("The operation has timed-out.", _fileException);

            }).ConfigureAwait(false);

            return ctx => Result.Set(ctx, new FileInfo(path));
        }

        private Exception _fileException;

        private Task<bool> ExecuteMainAsync(string path, bool waitForExist, int interval, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var done = false;
                if (!waitForExist && !File.Exists(path))
                    throw new FileNotFoundException(Resources.WaitFile_ErrorMsg_FilePathDoesNotExists, path);

                do
                {
                    try
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        if (!File.Exists(path))
                            throw new FileNotFoundException(Resources.WaitFile_ErrorMsg_FilePathDoesNotExists, path);

                        using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
                            done = true;
                    }
                    catch (Exception e)
                    {
                        done = e is OperationCanceledException || e is ObjectDisposedException;
                        _fileException = e;

                        if (!done)
                            Thread.Sleep(interval);
                    }
                } while (!done);

                return done;
            }, token);
        }
    }
}