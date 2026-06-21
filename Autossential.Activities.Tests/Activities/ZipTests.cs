using System.Activities;
using System.IO.Compression;
using System.Text;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class ZipTests
    {
        [Test]
        public async Task CompressDirectory_CreatesZipWithAllFiles()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            var zipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");

            try
            {
                var a = Path.Combine(dir, "a.txt");
                var b = Path.Combine(dir, "b.txt");
                var sub = Path.Combine(dir, "sub");
                Directory.CreateDirectory(sub);
                var c = Path.Combine(sub, "c.txt");

                File.WriteAllText(a, "a");
                File.WriteAllText(b, "b");
                File.WriteAllText(c, "c");

                var inputs = new Dictionary<string, object>
                {
                    ["ZipFilePath"] = zipPath,
                    ["ToCompress"] = new[] { dir },
                    ["TextEncoding"] = Encoding.UTF8
                };

                WorkflowInvoker.Invoke(new Zip(), inputs);

                await Assert.That(File.Exists(zipPath)).IsTrue();

                using var archive = ZipFile.OpenRead(zipPath);
                var filesCount = archive.Entries.Count(e => !e.FullName.EndsWith("/"));
                await Assert.That(filesCount).IsEqualTo(3);
            }
            finally
            {
                try { File.Delete(zipPath); } catch { }
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Test]
        public async Task CompressSingleFile_DoesNotIncludeZipFileItself()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            var zipPath = Path.Combine(dir, "target.zip");

            try
            {
                var file = Path.Combine(dir, "file.txt");
                File.WriteAllText(file, "content");

                var inputs = new Dictionary<string, object>
                {
                    ["ZipFilePath"] = zipPath,
                    ["ToCompress"] = new[] { file },
                    ["TextEncoding"] = Encoding.UTF8
                };

                WorkflowInvoker.Invoke(new Zip(), inputs);

                await Assert.That(File.Exists(zipPath)).IsTrue();

                using var archive = ZipFile.OpenRead(zipPath);
                var files = archive.Entries.Where(e => !e.FullName.EndsWith("/")).Select(e => e.Name).ToArray();

                await Assert.That(files.Length).IsEqualTo(1);
                await Assert.That(files).Contains("file.txt");
                await Assert.That(files).DoesNotContain(Path.GetFileName(zipPath));
            }
            finally
            {
                try { File.Delete(zipPath); } catch { }
                try { Directory.Delete(dir, true); } catch { }
            }
        }
    }
}