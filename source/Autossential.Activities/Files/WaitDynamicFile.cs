using Autossential.Core.Enums;
using Autossential.Core.Extensions;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.Collections.Generic;
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
        public InArgument<double> TimeoutSeconds { get; set; } = 30;
        public InArgument<DateTime?> FromDateTime { get; set; }
        public InArgument<double> IntervalSeconds { get; set; } = 0.5;
        public OutArgument<FileInfo> Result { get; set; }
        public InArgument<bool> FullPathMode { get; set; }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var dir = DirectoryPath.Get(context);
            var timeout = TimeoutSeconds.Get(context) * 1000;
            var searchPattern = SearchPattern.Get(context) ?? "*.*";
            var fromDateTime = FromDateTime.Get(context);
            var afterDate = fromDateTime ?? CalculateDate(dir);
            var interval = Math.Max(IntervalSeconds.Get(context), 0.5) * 1000;
            var fullPathMode = FullPathMode.Get(context);

            var filePath = await ExecuteWithTimeoutAsync(context, token, ExecuteMainAsync(dir, searchPattern, fullPathMode, afterDate, (int)interval, token), (int)timeout).ConfigureAwait(false);
            return ctx => Result.Set(ctx, filePath != null ? new FileInfo(filePath) : null);
        }

        private static DateTime CalculateDate(string dir)
        {
            var files = Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly);
            if (files.Any())
                return files.Max(path => File.GetLastWriteTime(path));

            return DateTime.Now;
        }

        private Task<string> ExecuteMainAsync(string dir, string searchPattern, bool fullPathMode, DateTime afterDate, int intervalMilliseconds, CancellationToken token)
        {
            Func<IEnumerable<string>> fn = fullPathMode
                ? () => Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories).Where(path => path.IsMatch(searchPattern))
                : () => Directory.EnumerateFiles(dir, "*", SearchOption.AllDirectories).Where(path => Path.GetFileName(path).IsMatch(searchPattern));

            return Task.Run(() =>
            {
                var done = false;

                try
                {
                    do
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        var files = fn().Where(path => File.GetLastWriteTime(path) > afterDate);

                        if (files.Any())
                        {
                            done = true;
                            return files.FirstOrDefault();
                        }

                        Thread.Sleep(intervalMilliseconds);

                    } while (!done);

                }
                catch (Exception e)
                {
                    done = e is OperationCanceledException || e is ObjectDisposedException;
                    if (!done)
                        Thread.Sleep(intervalMilliseconds);
                }

                return null;
            });
        }
    }
}