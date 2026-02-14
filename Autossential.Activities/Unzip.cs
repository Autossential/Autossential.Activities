using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autossential.Activities
{
    public sealed class Unzip : AsynchronousCodeActivity
    {
        public InArgument<string> ZipFilePath { get; set; }

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

                    foreach(var entry in zip.Entries)
                    {
                        if (token.IsCancellationRequested)
                            token.ThrowIfCancellationRequested();

                        var fullPath = Path.GetFullPath(Path.Combine(dirPath, entry.FullName));

                        //if (!fullPath.StartsWith(dirPath, StringComparison.OrdinalIgnoreCase))
                        //    throw new IOException(Resources.Unzip_ErrorMsg_OutsideDir);

                        if (Path.GetFileName(fullPath).Length == 0)
                        {
                            //if (entry.Length != 0L)
                            //    throw new IOException(Resources.Unzip_ErrorMsg_DirNameWithData);

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
