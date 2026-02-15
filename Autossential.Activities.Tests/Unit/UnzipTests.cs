using System.Activities;
using System.IO.Compression;
using System.Text;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    public class UnzipTests
    {
        [Fact]
        public void Invoke_Unzip_CreatesFilesAndDirectories()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");

            try
            {
                var a = Path.Combine(dir, "a.txt");
                var sub = Path.Combine(dir, "sub");
                Directory.CreateDirectory(sub);
                var c = Path.Combine(sub, "c.txt");

                File.WriteAllText(a, "a-content");
                File.WriteAllText(c, "c-content");

                // create zip from the directory
                ZipFile.CreateFromDirectory(dir, zipPath, CompressionLevel.Fastest, includeBaseDirectory: false, Encoding.UTF8);

                var extractTo = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                var inputs = new Dictionary<string, object>
                {
                    ["ZipFilePath"] = zipPath,
                    ["ExtractTo"] = extractTo,
                    ["Overwrite"] = true
                };

                WorkflowInvoker.Invoke(new Unzip(), inputs);

                var extractedA = Path.Combine(extractTo, "a.txt");
                var extractedC = Path.Combine(extractTo, "sub", "c.txt");

                Assert.True(File.Exists(extractedA));
                Assert.True(File.Exists(extractedC));
                Assert.Equal("a-content", File.ReadAllText(extractedA));
                Assert.Equal("c-content", File.ReadAllText(extractedC));
            }
            finally
            {
                try { File.Delete(zipPath); } catch { }
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Fact]
        public void Invoke_Unzip_OverwriteFalse_ThrowsWhenFileExists()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");

            try
            {
                var file = Path.Combine(dir, "file.txt");
                File.WriteAllText(file, "new-content");

                // create zip that contains file.txt with "new-content"
                ZipFile.CreateFromDirectory(dir, zipPath, CompressionLevel.Fastest, includeBaseDirectory: false, Encoding.UTF8);

                var extractTo = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(extractTo);

                var existing = Path.Combine(extractTo, "file.txt");
                File.WriteAllText(existing, "old-content");

                var inputs = new Dictionary<string, object>
                {
                    ["ZipFilePath"] = zipPath,
                    ["ExtractTo"] = extractTo,
                    ["Overwrite"] = false
                };

                Assert.Throws<System.IO.IOException>(() => WorkflowInvoker.Invoke(new Unzip(), inputs));

                // ensure original content still present
                Assert.Equal("old-content", File.ReadAllText(existing));
            }
            finally
            {
                try { File.Delete(zipPath); } catch { }
                try { Directory.Delete(dir, true); } catch { }
            }
        }
    }
}