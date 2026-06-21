using System.Activities;

namespace Autossential.Activities.Tests.Activities
{
    public class CleanUpFolderTests : BaseTests
    {

        [Test]
        public async Task RemovesAllFiles_RegardlessOfAttributes()
        {
            var dir = NewDir();
            var files = new Dictionary<string, FileAttributes>
                {
                    { "hidden.txt", FileAttributes.Hidden },
                    { "readonly.txt", FileAttributes.ReadOnly },
                    { "system.txt", FileAttributes.System },
                    { "archive.txt", FileAttributes.Archive },
                    { "temporary.txt", FileAttributes.Temporary },
                    { "normal.txt", FileAttributes.Normal },
                    { "offline.txt", FileAttributes.Offline },
                    { "encrypted.txt", FileAttributes.Encrypted },
                    { "compressed.txt", FileAttributes.Compressed },
                    { "notcontentindexed.txt", FileAttributes.NotContentIndexed }
                };

            foreach (var kvp in files)
            {
                var path = Path.Combine(dir, kvp.Key);
                File.WriteAllText(path, "...");
                File.SetAttributes(path, kvp.Value);
            }

            var inputs = new Dictionary<string, object>
            {
                ["Folder"] = dir,
                ["SearchPattern"] = "*.txt",
                ["DeleteEmptyFolders"] = true
            };

            WorkflowInvoker.Invoke(new CleanUpFolder(), inputs);

            await Assert.That(Directory.GetFiles(dir)).IsEmpty();
        }

        [Test]
        public async Task DeletesOldFilesAndEmptyFolders_WhenFolderHasFiles()
        {
            var dir = NewDir();

            // Create old file and new file
            var oldFile = Path.Combine(dir, "old.txt");
            File.WriteAllText(oldFile, "old");
            File.SetLastWriteTime(oldFile, DateTime.Now.AddDays(-10));

            var newFile = Path.Combine(dir, "new.txt");
            File.WriteAllText(newFile, "new");
            File.SetLastWriteTime(newFile, DateTime.Now);

            // Create an empty subfolder that should be deleted
            var emptyDir = Path.Combine(dir, "empty");
            Directory.CreateDirectory(emptyDir);

            var inputs = new Dictionary<string, object>
            {
                ["Folder"] = dir,
                ["LastWriteTime"] = DateTime.Now.AddDays(-1),
                ["SearchPattern"] = "*.txt",
                ["DeleteEmptyFolders"] = true
            };

            WorkflowInvoker.Invoke(new CleanUpFolder(), inputs);

            await Assert.That(File.Exists(oldFile)).IsFalse();
            await Assert.That(File.Exists(newFile)).IsTrue();
            await Assert.That(Directory.Exists(emptyDir)).IsFalse();
        }

        [Test]
        public async Task DeletesOnlyMatchingFiles_WithMultipleSearchPatterns()
        {
            var dir = NewDir();

            var aTxt = Path.Combine(dir, "a.txt");
            var bLog = Path.Combine(dir, "b.log");
            var cMd = Path.Combine(dir, "c.md");

            File.WriteAllText(aTxt, "a");
            File.WriteAllText(bLog, "b");
            File.WriteAllText(cMd, "c");

            File.SetLastWriteTime(aTxt, DateTime.Now.AddDays(-10));
            File.SetLastWriteTime(bLog, DateTime.Now.AddDays(-10));
            File.SetLastWriteTime(cMd, DateTime.Now.AddDays(-10));

            var inputs = new Dictionary<string, object>
            {
                ["Folder"] = dir,
                ["LastWriteTime"] = DateTime.Now.AddDays(-1),
                ["SearchPattern"] = "*.txt|*.md",
                ["DeleteEmptyFolders"] = false
            };

            WorkflowInvoker.Invoke(new CleanUpFolder(), inputs);

            await Assert.That(File.Exists(aTxt)).IsFalse();
            await Assert.That(File.Exists(bLog)).IsTrue();
            await Assert.That(File.Exists(cMd)).IsFalse();
        }

        [Test]
        public async Task ThrowsInvalidOperationException_WhenFolderArgumentIsNull()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Folder"] = null!,
                ["SearchPattern"] = "*.*"
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new CleanUpFolder(), inputs));
        }

        [Test]
        public async Task DoesNotDeleteFilesInSubdirectories_WithTopDirectoryOnly()
        {
            var dir = NewDir();

            var topFile = Path.Combine(dir, "top.txt");
            File.WriteAllText(topFile, "top");
            File.SetLastWriteTime(topFile, DateTime.Now.AddDays(-10));

            var sub = Path.Combine(dir, "sub");
            Directory.CreateDirectory(sub);
            var subFile = Path.Combine(sub, "sub.txt");
            File.WriteAllText(subFile, "sub");
            File.SetLastWriteTime(subFile, DateTime.Now.AddDays(-10));

            var inputs = new Dictionary<string, object>
            {
                ["Folder"] = dir,
                ["LastWriteTime"] = DateTime.Now.AddDays(-1),
                ["SearchPattern"] = "*.txt",
                ["DeleteEmptyFolders"] = false
            };

            WorkflowInvoker.Invoke(new CleanUpFolder { SearchOption = SearchOption.TopDirectoryOnly }, inputs);

            await Assert.That(File.Exists(topFile)).IsFalse();
            await Assert.That(File.Exists(subFile)).IsTrue();
        }

        [Test]
        public async Task DoesNotRemoveEmptyFolders_WhenDeleteEmptyFoldersFalse()
        {
            var dir = NewDir();

            var emptyDir = Path.Combine(dir, "empty");
            Directory.CreateDirectory(emptyDir);

            var inputs = new Dictionary<string, object>
            {
                ["Folder"] = dir,
                ["LastWriteTime"] = DateTime.Now,
                ["SearchPattern"] = "*.*",
                ["DeleteEmptyFolders"] = false
            };

            WorkflowInvoker.Invoke(new CleanUpFolder(), inputs);

            await Assert.That(Directory.Exists(emptyDir)).IsTrue();
        }
    }
}