using Autossential.Activities.Properties;
using Autossential.Shared;
using Autossential.Shared.Activities.Base;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class Zip : ContinuableAsyncTaskCodeActivity
    {
        public InArgument ToCompress { get; set; }

        [RequiredArgument]
        public InArgument<string> ZipFilePath { get; set; }

        public InArgument<Encoding> TextEncoding { get; set; }

        public CompressionLevel CompressionLevel { get; set; }

        public OutArgument<int> FilesCount { get; set; }

        public bool ShortEntryNames { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (ToCompress == null)
            {
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(ToCompress)));
            }
            else if (ToCompress.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(ToCompress, ToCompress.ArgumentType, nameof(ToCompress), true);
            }
            else
            {
                metadata.AddValidationError(Resources.Validation_TypeErrorFormat("IEnumerable<string> or IEnumerable<int>", nameof(ToCompress)));
            }
        }
        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var zipFilePath = Path.GetFullPath(ZipFilePath.Get(context));
            var toCompress = ToCompress.Get(context);
            var encoding = TextEncoding.Get(context);
            var counter = 0;

            if (toCompress is string)
                toCompress = new string[] { toCompress.ToString() };

            await Task.Run(() =>
            {
                var paths = (IEnumerable<string>)toCompress;
                var directories = paths.Where(Directory.Exists);
                var files = paths.Except(directories)
                     .Concat(directories.SelectMany(path => Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)))
                     .Select(Path.GetFullPath)
                     .Where(path => path != zipFilePath);

                var emptyFolders = directories.SelectMany(dir => Directory.EnumerateDirectories(dir, "*", SearchOption.AllDirectories))
                    .Select(Path.GetFullPath)
                    .Where(path => !Directory.EnumerateFileSystemEntries(path).Any());

                var entries = files.Concat(emptyFolders).OrderBy(path => path).ToArray();

                var mode = File.Exists(zipFilePath)
                    ? ZipArchiveMode.Update
                    : ZipArchiveMode.Create;

                using (var zip = ZipFile.Open(zipFilePath, mode, encoding))
                    counter = CompressTo(zip, entries, mode, token, null);

            }, token).ConfigureAwait(false);

            return ctx => FilesCount.Set(ctx, counter);
        }

        private int CompressTo(ZipArchive zip, string[] entries, ZipArchiveMode mode, CancellationToken token, string entryPrefix)
        {
            var commonDir = GetLongestCommonDir(entries);
            var prefixLen = commonDir.Length;
            var count = 0;

            if (prefixLen == 0)
            {
                foreach (var group in entries.GroupBy(Path.GetPathRoot))
                    count += CompressTo(zip, group.OrderBy(path => path).ToArray(), mode, token, group.Key.Replace(":", ""));

                return count;
            }

            void Add(string path, string name)
            {
                if (File.Exists(path))
                {
                    zip.CreateEntryFromFile(path, entryPrefix + name);
                }
                else if (Directory.Exists(path))
                {
                    if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                        zip.CreateEntry(entryPrefix + path);
                    else
                        zip.CreateEntry(entryPrefix + path + Path.DirectorySeparatorChar);
                }
            }

            string EnsureUnique(string name)
            {
                if (mode == ZipArchiveMode.Update)
                    zip.GetEntry(name)?.Delete();

                return name;
            }

            if (ShortEntryNames || entryPrefix == null)
            {
                foreach (var fullPath in entries)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    Add(fullPath, EnsureUnique(fullPath.Substring(prefixLen)));
                }
            }
            else
            {
                if (entryPrefix != null)
                    prefixLen = entryPrefix.Length;

                foreach (var fullPath in entries)
                {
                    Add(fullPath, EnsureUnique(fullPath.Replace(":", "").Substring(prefixLen)));
                }
            }

            return count;
        }

        private string GetLongestCommonDir(string[] files)
        {
            var size = files.Length;
            if (size == 1) return Path.GetDirectoryName(files[0]) + "\\";

            var first = files[0].Split(Path.DirectorySeparatorChar);
            var last = files[files.Length - 1].Split(Path.DirectorySeparatorChar);

            var min = Math.Min(first.Length, last.Length);

            int i = 0, j = 0;
            while (i < min && first[i] == last[i])
            {
                j += first[i].Length + 1;
                i++;
            }

            return files[0].Substring(0, j);
        }
    }
}