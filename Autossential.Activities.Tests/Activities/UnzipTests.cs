using System.Activities;
using System.IO.Compression;
using System.Text;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class UnzipTests:BaseTests
    {
        [Test]
        public async Task Unzip_CreatesFilesAndDirectories()
        {
            var dir = NewDir();
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

                await Assert.That(File.Exists(extractedA)).IsTrue();
                await Assert.That(File.Exists(extractedC)).IsTrue();
                await Assert.That(File.ReadAllText(extractedA)).IsEqualTo("a-content");
                await Assert.That(File.ReadAllText(extractedC)).IsEqualTo("c-content");
            }
            finally
            {
                try { File.Delete(zipPath); } catch { }
            }
        }

        [Test]
        public async Task Unzip_OverwriteFalse_ThrowsWhenFileExists()
        {
            var dir = NewDir();
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

                await Assert.That(() => WorkflowInvoker.Invoke(new Unzip(), inputs))
                    .Throws<System.IO.IOException>();

                // ensure original content still present
                await Assert.That(File.ReadAllText(existing)).IsEqualTo("old-content");
            }
            finally
            {
                try { File.Delete(zipPath); } catch { }
            }
        }
    }
}