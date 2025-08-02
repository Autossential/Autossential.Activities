using Autossential.Activities.Properties;
using Autossential.Core.Extensions;
using Autossential.Core.Models;
using Autossential.Shared;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class CleanUpFolder : ContinuableAsyncTaskCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> Folder { get; set; }
        public InArgument SearchPattern { get; set; }
        public OutArgument<CleanUpFolderResult> Result { get; set; }
        public InArgument<DateTime?> LastWriteTime { get; set; }
        public InArgument<bool> DeleteEmptyFolders { get; set; } = true;
        public SearchOption SearchOption { get; set; } = SearchOption.AllDirectories;
        public InArgument<bool> FullPathMode { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (SearchPattern == null) return;

            if (SearchPattern.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(SearchPattern, SearchPattern.ArgumentType, nameof(SearchPattern), false);
                return;
            }

            metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("string or collection of strings", nameof(SearchPattern)));
        }

        protected async override Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var folder = Folder.Get(context);
            var patterns = SearchPattern?.GetAsHashSet<string>(context) ?? new HashSet<string>(["*"]);

            var lastWriteTime = LastWriteTime?.Get(context) ?? DateTime.Now;
            var deleteEmptyFolders = DeleteEmptyFolders.Get(context);

            var fullPathMode = FullPathMode.Get(context);

            int filesDeleted = 0;
            int foldersDeleted = 0;

            await Task.Run(() =>
            {
                foreach (var pattern in patterns)
                {
                    var files = fullPathMode
                        ? Directory.EnumerateFiles(folder, "*", SearchOption).Where(path => path.IsMatch(pattern))
                        : Directory.EnumerateFiles(folder, "*", SearchOption).Where(path => Path.GetFileName(path).IsMatch(pattern));

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
                        catch (Exception e)
                        {
                            Trace.WriteLine($"{f}: {e.Message}");
                        }
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
                        catch (Exception e)
                        {
                            Trace.WriteLine($"{f}: {e.Message}");
                        }
                    }
                }
            }, token).ConfigureAwait(false);

            return ctx =>
            {
                Result.Set(ctx, new CleanUpFolderResult
                {
                    FilesDeleted = filesDeleted,
                    FoldersDeleted = foldersDeleted
                });
            };
        }
    }
}