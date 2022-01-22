using Autossential.Activities.Properties;
using Autossential.Shared;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class WaitDynamicFile : ContinuableAsyncTaskCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> DirectoryPath { get; set; }
        public InArgument<string> SearchPattern { get; set; }
        public InArgument<int> Timeout { get; set; } = 30000;
        public int Interval { get; set; } = 500;
        public OutArgument<FileInfo> Result { get; set; }

        private const int MINIMUM_INTERVAL = 100;
        private const int MAXIMUM_INTERVAL = 20000;

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Interval < MINIMUM_INTERVAL || Interval > MAXIMUM_INTERVAL)
                metadata.AddValidationWarning(Resources.WaitDynamicFile_ErrorMsg_IntervalRangeFormat(MINIMUM_INTERVAL, MAXIMUM_INTERVAL));
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var dir = DirectoryPath.Get(context);
            var time = Timeout.Get(context);
            var searchPattern = SearchPattern.Get(context) ?? "*.*";
            var afterDate = CalculateDate(dir);
            var filePath = await ExecuteWithTimeoutAsync(context, token, ExecuteMainAsync(dir, searchPattern, afterDate, token), time).ConfigureAwait(false);
            return ctx => Result.Set(ctx, filePath != null ? new FileInfo(filePath) : null);
        }

        private DateTime CalculateDate(string dir)
        {
            var files = Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly);
            if (files.Any())
                return files.Max(path => File.GetLastWriteTime(path));

            return DateTime.Now;
        }

        private Task<string> ExecuteMainAsync(string dir, string searchPattern, DateTime afterDate, CancellationToken token)
        {
            var interval = GetInterval();
            return Task.Run(() =>
            {
                var done = false;

                try
                {
                    do
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        var files = Directory.EnumerateFiles(dir, searchPattern).Where(path => File.GetCreationTime(path) > afterDate);
                        if (files.Any())
                        {
                            done = true;
                            return files.FirstOrDefault();
                        }

                        Thread.Sleep(interval);

                    } while (!done);

                }
                catch (Exception e)
                {
                    done = e is OperationCanceledException || e is ObjectDisposedException;
                    if (!done)
                        Thread.Sleep(interval);
                }

                return null;
            });
        }

        private int GetInterval()
        {
            return Interval < MINIMUM_INTERVAL ? MINIMUM_INTERVAL : Math.Min(Interval, MAXIMUM_INTERVAL);
        }
    }
}