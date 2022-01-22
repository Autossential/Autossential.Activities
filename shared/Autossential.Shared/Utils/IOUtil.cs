using System;
using System.IO;

namespace Autossential.Shared.Utils
{
    public static class IOUtil
    {
        public static string GetRelativePath(string basePath, string path)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                return path;

            try
            {
                var workspaceUri = new Uri(Path.Combine(basePath, Path.PathSeparator.ToString()));
                var workflowUri = new Uri(path);

                if (workspaceUri.IsBaseOf(workflowUri))
                    path = Uri.UnescapeDataString(workspaceUri.MakeRelativeUri(workflowUri).OriginalString).Replace('/', Path.DirectorySeparatorChar);
            }
            catch { }

            return path;
        }
    }
}