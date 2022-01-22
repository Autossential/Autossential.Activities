using Autossential.Activities.Properties;
using Autossential.Shared;
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

        public bool WaitForExist { get; set; }

        public int Interval { get; set; } = 500;

        public OutArgument<FileInfo> Result { get; set; }

        private const int MINIMUM_INTERVAL = 100;
        private const int MAXIMUM_INTERVAL = 20000;

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Interval < MINIMUM_INTERVAL || Interval > MAXIMUM_INTERVAL)
                metadata.AddValidationWarning(Resources.WaitFile_ErrorMsg_IntervalRangeFormat(MINIMUM_INTERVAL, MAXIMUM_INTERVAL));
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var path = FilePath.Get(context);
            var time = Timeout.Get(context);

            await ExecuteWithTimeoutAsync(context, token, ExecuteMainAsync(token, path), time, defaultHandler =>
            {
                throw new TimeoutException("The operation has timed-out.", _fileException);

            }).ConfigureAwait(false);

            return ctx => Result.Set(ctx, new FileInfo(path));
        }

        private Exception _fileException;

        private Task<bool> ExecuteMainAsync(CancellationToken token, string path)
        {
            var interval = GetInterval();

            return Task.Run(() =>
            {
                var done = false;
                if (!WaitForExist && !File.Exists(path))
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

        private int GetInterval()
        {
            return Interval < MINIMUM_INTERVAL ? MINIMUM_INTERVAL : Math.Min(Interval, MAXIMUM_INTERVAL);
        }
    }
}