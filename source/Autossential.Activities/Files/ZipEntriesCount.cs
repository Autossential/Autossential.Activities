using Autossential.Activities.Properties;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class ZipEntriesCount : ContinuableAsyncTaskCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> ZipFilePath { get; set; }

        public OutArgument<int> EntriesCount { get; set; }

        public OutArgument<int> FilesCount { get; set; }

        public OutArgument<int> FoldersCount { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (EntriesCount == null && FilesCount == null && FoldersCount == null)
                metadata.AddValidationError(Resources.ZipEntriesCount_ErrorMsg_OutputMissing);

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var entriesCount = 0;
            var foldersCount = 0;
            var filesCount = 0;
            var filePath = ZipFilePath.Get(context);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            await Task.Run(() =>
            {
                using (var zip = ZipFile.Open(filePath, ZipArchiveMode.Read))
                {
                    entriesCount = zip.Entries.Count;
                    foldersCount = zip.Entries.Count(entry => string.IsNullOrEmpty(entry.Name));
                    filesCount = entriesCount - foldersCount;
                }
            }).ConfigureAwait(false);

            return ctx =>
            {
                EntriesCount.Set(ctx, entriesCount);
                FilesCount.Set(ctx, filesCount);
                FoldersCount.Set(ctx, foldersCount);
            };
        }
    }
}