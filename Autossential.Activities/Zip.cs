using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System.Activities;
using System.IO.Compression;
using System.Text;

namespace Autossential.Activities
{
    public sealed class Zip : AsyncActivity
    {
        public enum ZipEntryStructure
        {
            RelativePaths,           // mantém relativo ao common dir (comportamento atual)
            Flatten,                 // tudo na raiz (erro se duplicado)
            FlattenWithRename,       // tudo na raiz, renomeando duplicados
            MirrorDirectoryStructure,// preserva caminho completo
            Auto                     // inteligente: flat se possível, senão relativo
        }

        private enum ZipMode
        {
            Relative,
            Flat,
            FlatRename,
            Mirror
        }

        [RequiredArgument]
        public InArgument<string> ZipFilePath { get; set; }
        [RequiredArgument]
        public InArgument<IReadOnlyList<string>> ToCompress { get; set; }
        public InArgument<Encoding> TextEncoding { get; set; }
        public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Optimal;
        public ZipEntryStructure EntryStructure { get; set; } = ZipEntryStructure.RelativePaths;
        public OutArgument<FileInfo> CompressedFile { get; set; }
        public OutArgument<int> FilesCount { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (ToCompress == null)
                metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Zip_ToCompress_DisplayName));

            if (ZipFilePath == null)
                metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Zip_ZipFilePath_DisplayName));
        }

        protected override Task<Action<AsyncCodeActivityContext>> RunAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var zipFilePath = Path.GetFullPath(ZipFilePath.Get(context)) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Zip_ZipFilePath_DisplayName));
            var toCompress = ToCompress.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Zip_ToCompress_DisplayName));
            var encoding = TextEncoding.Get(context) ?? Encoding.UTF8;

            return Task.Run<Action<AsyncCodeActivityContext>>(() =>
            {
                var items = toCompress
                    .Where(Directory.Exists)
                    .SelectMany(path => Directory.EnumerateFileSystemEntries(path, "*", SearchOption.AllDirectories));

                var files = toCompress
                    .Select(Path.GetFullPath)
                    .Where(File.Exists);

                items = items
                    .Union(files)
                    .Where(path => path != Path.GetFullPath(zipFilePath))
                    .OrderBy(path => path);

                var allEntries = items.ToArray();

                var duplicates = allEntries
                    .Where(File.Exists)
                    .GroupBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Count() > 1)
                    .ToList();

                var hasDuplicates = duplicates.Any();

                if (EntryStructure == ZipEntryStructure.Flatten && hasDuplicates)
                {
                    var details = string.Join(Environment.NewLine,
                        duplicates.Select(g =>
                            $"{g.Key}:{Environment.NewLine} - {string.Join(Environment.NewLine + " - ", g)}"));

                    throw new InvalidOperationException(
                        $"Cannot flatten entries because duplicate file names were found:{Environment.NewLine}{details}");
                }

                var effectiveMode = EntryStructure switch
                {
                    ZipEntryStructure.Flatten => ZipMode.Flat,
                    ZipEntryStructure.FlattenWithRename => ZipMode.FlatRename,
                    ZipEntryStructure.RelativePaths => ZipMode.Relative,
                    ZipEntryStructure.MirrorDirectoryStructure => ZipMode.Mirror,
                    ZipEntryStructure.Auto => hasDuplicates ? ZipMode.Relative : ZipMode.Flat,
                    _ => ZipMode.Relative
                };

                var filesCount = 0;

                using (var zip = ZipFile.Open(zipFilePath,
                    File.Exists(zipFilePath) ? ZipArchiveMode.Update : ZipArchiveMode.Create,
                    encoding))
                {
                    filesCount = CompressTo(zip, allEntries, null, effectiveMode, token);
                }

                return ctx =>
                {
                    CompressedFile.Set(ctx, new FileInfo(zipFilePath));
                    FilesCount.Set(ctx, filesCount);
                };
            }, token);
        }

        private static int CompressTo(
           ZipArchive zip,
           string[] entries,
           string prefix,
           ZipMode mode,
           CancellationToken token)
        {
            var commonDir = GetLongestCommonDir(entries);
            var filesCount = 0;

            if (commonDir.Length == 0 && mode == ZipMode.Relative)
            {
                foreach (var group in entries.GroupBy(Path.GetPathRoot))
                {
                    filesCount += CompressTo(
                        zip,
                        [.. group.OrderBy(path => path)],
                        group.Key.Replace(":", ""),
                        mode,
                        token);
                }

                return filesCount;
            }

            var prefixLen = Math.Max(commonDir.Length, prefix?.Length ?? 0);

            var nameCounter = mode == ZipMode.FlatRename
                ? new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                : null;

            foreach (var path in entries)
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();

                string entryName;

                switch (mode)
                {
                    case ZipMode.Mirror:
                        entryName = path.Replace(":", "");
                        break;

                    case ZipMode.Flat:
                        if (!File.Exists(path)) continue;
                        entryName = Path.GetFileName(path);
                        break;

                    case ZipMode.FlatRename:
                        if (!File.Exists(path)) continue;

                        var fileName = Path.GetFileName(path);

                        if (!nameCounter.TryGetValue(fileName, out var count))
                        {
                            nameCounter[fileName] = 0;
                            entryName = fileName;
                        }
                        else
                        {
                            count++;
                            nameCounter[fileName] = count;

                            var name = Path.GetFileNameWithoutExtension(fileName);
                            var ext = Path.GetExtension(fileName);

                            entryName = $"{name} ({count}){ext}";
                        }
                        break;

                    default: // Relative
                        entryName = prefix + path[prefixLen..];
                        break;
                }

                if (File.Exists(path))
                {
                    zip.CreateEntryFromFile(path, entryName);
                    filesCount++;
                }
                else if (mode == ZipMode.Relative || mode == ZipMode.Mirror)
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