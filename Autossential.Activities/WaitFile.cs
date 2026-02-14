using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class WaitFile : AsyncTimeoutCodeActivity<FileInfo>
    {
        public InArgument<string> FilePath { get; set; }
        public InArgument<double> PollingIntervalSeconds { get; set; } = 0.5;
        public InArgument<bool> WaitForExist { get; set; } = true;
        public InArgument<string> DirectoryPath { get; set; }
        public InArgument<string> SearchPattern { get; set; }
        public bool DynamicFile { get; set; } = false;

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (DynamicFile)
            {
                if (DirectoryPath == null || DirectoryPath.Expression == null)
                    metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.WaitFile_DirectoryPath_DisplayName));
            }
            else
            {
                if (FilePath == null || FilePath.Expression == null)
                    metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.WaitFile_FilePath_DisplayName));
            }
        }

        protected override Task<FileInfo> RunAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            if (DynamicFile)
                return WaitForDynamicFile(context, token);

            return WaitForFile(context, token);
        }

        private Task<FileInfo> WaitForDynamicFile(AsyncCodeActivityContext context, CancellationToken token)
        {
            var dirPath = DirectoryPath.Get(context) ?? throw new NullReferenceException(nameof(DirectoryPath));
            var searchPattern = SearchPattern.Get(context) ?? "*.*";
            var pollingInterval = TimeSpan.FromSeconds(Math.Max(PollingIntervalSeconds.Get(context), 0.1)); // Minimum 0.1 seconds
            string filePath = null;

            if (!WaitForExist.Get(context) && !Directory.EnumerateFiles(dirPath, "*", SearchOption.TopDirectoryOnly).Any(path => Path.GetFileName(path).IsMatch(searchPattern)))
                throw new FileNotFoundException(Resources.WaitFile_ErrorMsg_NoFileFound);

            return Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        if (filePath == null)
                        {
                            filePath = Directory
                                .EnumerateFiles(dirPath, "*", SearchOption.TopDirectoryOnly)
                                .FirstOrDefault(path => Path.GetFileName(path).IsMatch(searchPattern));
                        }
                        else
                        {
                            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                            break;
                        }


                        Thread.Sleep(pollingInterval);
                        break;
                    }
                    catch (Exception e)
                    {
                        if (e is OperationCanceledException || e is ObjectDisposedException)
                            break;
                        else
                            Thread.Sleep(pollingInterval);
                    }
                }

                return filePath == null ? null : new FileInfo(filePath);
            }, token);
        }

        private Task<FileInfo> WaitForFile(AsyncCodeActivityContext context, CancellationToken token)
        {
            var filePath = FilePath.Get(context) ?? throw new NullReferenceException(nameof(FilePath));
            var pollingInterval = TimeSpan.FromSeconds(Math.Max(PollingIntervalSeconds.Get(context), 0.1)); // Minimum 0.1 seconds

            if (!WaitForExist.Get(context) && !File.Exists(filePath))
                throw new FileNotFoundException(Resources.WaitFile_ErrorMsg_NoFileFound);

            return Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                        break;
                    }
                    catch (Exception e)
                    {
                        if (e is OperationCanceledException || e is ObjectDisposedException)
                            break;
                        else
                            Thread.Sleep(pollingInterval);
                    }
                }

                return new FileInfo(filePath);

            }, token);
        }
    }
}
