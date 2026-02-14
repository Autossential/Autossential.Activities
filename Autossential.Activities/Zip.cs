using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System.Activities;
using System.IO.Compression;
using System.Text;

namespace Autossential.Activities
{
    public sealed class Zip : AsynchronousCodeActivity
    {
        public InArgument<string> ZipFilePath { get; set; }
        public InArgument<IEnumerable<string>> ToCompress { get; set; }
        public InArgument<Encoding> TextEncoding { get; set; }
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;
        public InArgument<bool> FullEntryNames { get; set; } = false;
        public OutArgument<FileInfo> CompressedFile { get; set; }
        public OutArgument<int> FilesCount { get; set; }

        override protected void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (ToCompress == null)
                metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Zip_ToCompress_DisplayName));

            if (ZipFilePath == null)
                metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Zip_ZipFilePath_DisplayName));
        }

        protected override Task<Action<AsyncCodeActivityContext>> RunAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var zipFilePath = Path.GetFullPath(ZipFilePath.Get(context));
            var toCompress = ToCompress.Get(context);
            var encoding = TextEncoding.Get(context);
            var fullEntryNames = FullEntryNames.Get(context);

            return Task.Run<Action<AsyncCodeActivityContext>>(() =>
            {
                var items = toCompress.Where(Directory.Exists).SelectMany(path => Directory.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories));
                var files = toCompress.Select(Path.GetFullPath).Where(File.Exists);

                items = items.Union(files).Where(path => path != Path.GetFullPath(zipFilePath)).OrderBy(path => path);

                var filesCount = 0;
                using (var zip = ZipFile.Open(zipFilePath, File.Exists(zipFilePath) ? ZipArchiveMode.Update : ZipArchiveMode.Create, encoding))
                {
                    filesCount = CompressTo(zip, [.. items], null, fullEntryNames, token);
                }

                return ctx =>
                    {
                        CompressedFile.Set(ctx, new FileInfo(zipFilePath));
                        FilesCount.Set(ctx, filesCount);
                    };
            }, token);
        }

        private static int CompressTo(ZipArchive zip, string[] entries, string prefix, bool fullEntryNames, CancellationToken token)
        {
            var commonDir = GetLongestCommonDir(entries);
            var filesCount = 0;

            if (commonDir.Length == 0)
            {
                foreach (var group in entries.GroupBy(Path.GetPathRoot))
                    filesCount += CompressTo(zip, [.. group.OrderBy(path => path)], group.Key.Replace(":", ""), fullEntryNames, token);

                return filesCount;
            }

            var prefixLen = Math.Max(commonDir.Length, prefix?.Length ?? 0);
            foreach (var path in entries)
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                var entryName = fullEntryNames ? path.Replace(":", "") : prefix + path[prefixLen..];

                if (File.Exists(path))
                {
                    zip.CreateEntryFromFile(path, entryName);
                    filesCount++;
                }
                else
                {
                    zip.CreateEntry(entryName.TrimEnd(['\\', '/']) + '/');
                }
            }

            return filesCount;
        }

        private static string GetLongestCommonDir(string[] files)
        {
            var size = files.Length;
            if (size == 1) return Path.GetDirectoryName(files[0]) + "\\";

            var first = files[0].Split(Path.DirectorySeparatorChar);
            var last = files[^1].Split(Path.DirectorySeparatorChar);

            var min = Math.Min(first.Length, last.Length);

            int i = 0, j = 0;
            while (i < min && first[i] == last[i])
            {
                j += first[i].Length + 1;
                i++;
            }

            return files[0][..j];
        }
    }
}