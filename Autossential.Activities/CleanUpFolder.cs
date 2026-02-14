using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class CleanUpFolder : AsyncTimeoutCodeActivity
    {
        public InArgument<string> Folder { get; set; }
        public InArgument<string> SearchPattern { get; set; }
        public OutArgument<int> FilesDeleted { get; set; }
        public OutArgument<int> FoldersDeleted { get; set; }
        public InArgument<DateTime?> LastWriteTime { get; set; }
        public InArgument<bool> DeleteEmptyFolders { get; set; } = true;
        public SearchOption SearchOption { get; set; } = SearchOption.AllDirectories;

        override protected void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (Folder == null)
                metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.CleanUpFolder_Folder_DisplayName));
        }
        protected override Task<Action<AsyncCodeActivityContext>> RunAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var folder = Folder.Get(context) ?? throw new NullReferenceException(nameof(Folder));
            var searchPatterns = new HashSet<string>((SearchPattern.Get(context) ?? "*.*").Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
            var lastWriteTime = LastWriteTime.Get(context) ?? DateTime.Now;
            var deleteEmptyFolders = DeleteEmptyFolders.Get(context);

            return Task.Run<Action<AsyncCodeActivityContext>>(() =>
            {
                var filesDeleted = 0;
                var foldersDeleted = 0;

                foreach (var pattern in searchPatterns)
                {
                    var files = Directory.EnumerateFiles(folder, "*", SearchOption).Where(path => Path.GetFileName(path).IsMatch(pattern));
                    foreach (var f in files.Reverse())
                    {
                        try
                        {
                            if (token.IsCancellationRequested)
                                break;

                            if (File.GetLastWriteTime(f) > lastWriteTime)
                                continue;

                            File.Delete(f);
                            filesDeleted++;
                        }
                        catch { }
                    }
                }

                if (deleteEmptyFolders)
                {
                    foreach (var f in Directory.EnumerateDirectories(folder, "*", SearchOption).Reverse())
                    {
                        if (token.IsCancellationRequested)
                            break;

                        if (Directory.EnumerateFileSystemEntries(f, "*").Any())
                            continue;

                        try
                        {
                            Directory.Delete(f);
                            foldersDeleted++;
                        }
                        catch { }
                    }
                }

                return ctx =>
                {
                    FilesDeleted.Set(ctx, filesDeleted);
                    FoldersDeleted.Set(ctx, foldersDeleted);
                };

            }, token);
        }
    }
}