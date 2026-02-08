using System.Activities;

namespace Autossential.Activities
{
    public sealed class WaitDynamicFile : AsyncTimeoutCodeActivity<FileInfo>
    {
        public InArgument<string> DirectoryPath { get; set; }
        public InArgument<string> SearchPattern { get; set; }
        public InArgument<DateTime?> FromDateTime { get; set; }
        protected override Task<FileInfo> RunAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var dirPath = DirectoryPath.Get(context) ?? throw new NullReferenceException(nameof(DirectoryPath));
            var searchPattern = SearchPattern.Get(context) ?? "*.*";
            var fromDateTime = FromDateTime.Get(context);
            var afterDate = fromDateTime ?? CalculateDate(dirPath);

            return Task.Run(() =>
            {
                var files = Directory.EnumerateFiles(dirPath, "*", SearchOption.AllDirectories)
                    .Where(path => Path.GetFileName(path).IsMatch(searchPattern))
                    .Where(path => File.GetLastWriteTime(path) > afterDate);
                if (files.Any())
                {
                    var filePath = files.FirstOrDefault();
                    return new FileInfo(filePath);
                }
                return null;
            }, token);
        }
        private static DateTime CalculateDate(string dir)
        {
            var files = Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly);
            if (files.Any())
                return files.Max(path => File.GetLastWriteTime(path));

            return DateTime.Now;
        }
    }
}

/*
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
    }*/