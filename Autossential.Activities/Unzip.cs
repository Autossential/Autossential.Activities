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

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (ZipFilePath == null)
                metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Unzip_ZipFilePath_DisplayName));

            if (ExtractTo == null)
                metadata.AddValidationError(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.Unzip_ExtractTo_DisplayName));
        }

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
