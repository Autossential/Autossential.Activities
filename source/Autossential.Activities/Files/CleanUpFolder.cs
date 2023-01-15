using Autossential.Activities.Properties;
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

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (SearchPattern == null) return;

            if (SearchPattern.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(SearchPattern, SearchPattern.ArgumentType, nameof(SearchPattern), false);
                return;
            }

            metadata.AddValidationError(Resources.Validation_TypeErrorFormat("string or collection of strings", nameof(SearchPattern)));
        }

        protected async override Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var folder = Folder.Get(context);
            var patterns = SearchPattern?.GetAsArray<string>(context) ?? new[] { "*" };
            var lastWriteTime = LastWriteTime?.Get(context) ?? DateTime.Now;
            var deleteEmptyFolders = DeleteEmptyFolders.Get(context);

            int filesDeleted = 0;
            int foldersDeleted = 0;

            await Task.Run(() =>
            {
                foreach (var p in patterns)
                {
                    foreach (var f in Directory.EnumerateFiles(folder, p, SearchOption.AllDirectories).Reverse())
                    {
                        try
                        {
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
                    foreach (var f in Directory.EnumerateDirectories(folder, "*", SearchOption.AllDirectories).Reverse())
                    {
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
            }).ConfigureAwait(false);

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