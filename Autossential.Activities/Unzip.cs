using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System.Activities;
using System.IO.Compression;

namespace Autossential.Activities
{
    public sealed class Unzip : AsyncActivity
    {
        [RequiredArgument]
        public InArgument<string> ZipFilePath { get; set; }

        [RequiredArgument]
        public InArgument<string> ExtractTo { get; set; }

        public InArgument<bool> Overwrite { get; set; }

        protected override Task<Action<AsyncCodeActivityContext>> RunAsync(AsyncCodeActivityContext context, CancellationToken token)
        {
            var zipFilePath = ZipFilePath.Get(context);
            var extractTo = ExtractTo.Get(context);
            var overwrite = Overwrite.Get(context);

            return Task.Run<Action<AsyncCodeActivityContext>>(() =>
            {
                using (var zip = ZipFile.OpenRead(zipFilePath))
                {
                    var dir = Directory.CreateDirectory(extractTo);
                    var dirPath = dir.FullName;

                    foreach (var entry in zip.Entries)
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        var fullPath = Path.GetFullPath(Path.Combine(dirPath, entry.FullName));

                        if (Path.GetFileName(fullPath).Length == 0)
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                        else
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                            entry.ExtractToFile(fullPath, overwrite);
                        }
                    }
                }
                return _ => { };
            }, token);
        }
    }
}
