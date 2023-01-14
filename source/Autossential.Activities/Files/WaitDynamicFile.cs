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
        public InArgument<DateTime?> FromDateTime { get; set; }
        public InArgument<int> Interval { get; set; } = 500;
        public OutArgument<FileInfo> Result { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var dir = DirectoryPath.Get(context);
            var timeout = Timeout.Get(context);
            var searchPattern = SearchPattern.Get(context) ?? "*.*";
            var fromDateTime = FromDateTime.Get(context);
            var afterDate = (fromDateTime == null) ? CalculateDate(dir) : fromDateTime.Value;
            var interval = Math.Max(Interval.Get(context), 50);

            var filePath = await ExecuteWithTimeoutAsync(context, token, ExecuteMainAsync(dir, searchPattern, afterDate, interval, token), timeout).ConfigureAwait(false);
            return ctx => Result.Set(ctx, filePath != null ? new FileInfo(filePath) : null);
        }

        private DateTime CalculateDate(string dir)
        {
            var files = Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly);
            if (files.Any())
                return files.Max(path => File.GetLastWriteTime(path));

            return DateTime.Now;
        }

        private Task<string> ExecuteMainAsync(string dir, string searchPattern, DateTime afterDate, int interval, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var done = false;

                try
                {
                    do
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        var files = Directory.EnumerateFiles(dir, searchPattern).Where(path => File.GetLastWriteTime(path) > afterDate);
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
    }
}