using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    public class CleanUpFolderTests
    {
        [Fact]
        public void Invoke_WhenFolderHasFiles_DeletesOldFilesAndEmptyFolders()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            try
            {
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

                var result = WorkflowInvoker.Invoke(new CleanUpFolder(), inputs);

                // The activity sets out arguments via the context; WorkflowInvoker returns null for activities that use out args.
                // Retrieve out arguments by invoking the activity through WorkflowInvoker and reading the outputs dictionary is not straightforward here,
                // so verify file system side-effects instead.
                Assert.False(File.Exists(oldFile));
                Assert.True(File.Exists(newFile));
                Assert.False(Directory.Exists(emptyDir));
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Fact]
        public void Invoke_WhenFolderArgumentIsNull_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Folder"] = null!,
                ["SearchPattern"] = "*.*"
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new CleanUpFolder(), inputs));
        }

        [Fact]
        public void Invoke_WithMultipleSearchPatterns_DeletesOnlyMatchingFiles()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            try
            {
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

                Assert.False(File.Exists(aTxt));
                Assert.True(File.Exists(bLog));
                Assert.False(File.Exists(cMd));
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Fact]
        public void Invoke_WithTopDirectoryOnly_DoesNotDeleteFilesInSubdirectories()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            try
            {
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

                Assert.False(File.Exists(topFile));
                Assert.True(File.Exists(subFile));
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Fact]
        public void Invoke_WhenDeleteEmptyFoldersFalse_DoesNotRemoveEmptyFolders()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            try
            {
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

                Assert.True(Directory.Exists(emptyDir));
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }
    }
}
