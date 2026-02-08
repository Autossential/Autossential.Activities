using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    public class WaitFileTests
    {    
        [Fact]
        public void Invoke_WhenFileAccessible_ReturnsFileInfo()
        {
            var path = Path.GetTempFileName();
            try
            {
                File.WriteAllText(path, "content");

                var inputs = new Dictionary<string, object>
                {
                    ["FilePath"] = path,
                    ["TimeoutSeconds"] = 5.0
                };

                var result = WorkflowInvoker.Invoke(new WaitFile(), inputs);
                var info = Assert.IsType<FileInfo>(result);
                Assert.Equal(Path.GetFullPath(path), info.FullName);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void Invoke_WhenFileLocked_TimesOut()
        {
            var path = Path.GetTempFileName();
            try
            {
                File.WriteAllText(path, "content");

                // Open the file with FileShare.None so WaitFile cannot open it.
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);

                var inputs = new Dictionary<string, object>
                {
                    ["FilePath"] = path,
                    ["TimeoutSeconds"] = 1.0,             // short timeout to keep test fast
                    ["PollingIntervalSeconds"] = 0.5
                };

                Assert.Throws<TimeoutException>(() => WorkflowInvoker.Invoke(new WaitFile(), inputs));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void Invoke_WhenFilePathIsNull_ThrowsArgumentNullException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = null!,
                ["TimeoutSeconds"] = 1.0
            };

            // The activity throws NullReferenceException when the FilePath argument is null
            Assert.Throws<NullReferenceException>(() => WorkflowInvoker.Invoke(new WaitFile(), inputs));
        }

        [Fact]
        public void Invoke_DynamicFile_WhenFilePresent_ReturnsFileInfo()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);
            try
            {
                var file = Path.Combine(dir, "test.txt");
                File.WriteAllText(file, "content");

                var inputs = new Dictionary<string, object>
                {
                    ["DirectoryPath"] = dir,
                    ["SearchPattern"] = "test.txt",
                    ["TimeoutSeconds"] = 5.0,
                    ["PollingIntervalSeconds"] = 0.1
                };

                var result = WorkflowInvoker.Invoke(new WaitFile { DynamicFile = true }, inputs);
                var info = Assert.IsType<FileInfo>(result);
                Assert.Equal(Path.GetFullPath(file), info.FullName);
            }
            finally
            {
                Directory.Delete(dir, true);
            }
        }

        [Fact]
        public void Invoke_DynamicFile_WhenDirectoryPathIsNull_ThrowsNullReferenceException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DirectoryPath"] = null!,
                ["TimeoutSeconds"] = 1.0
            };

            Assert.Throws<NullReferenceException>(() => WorkflowInvoker.Invoke(new WaitFile { DynamicFile = true }, inputs));
        }

        [Fact]
        public void Invoke_WhenWaitForExistFalse_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");
            if (File.Exists(path))
                File.Delete(path);

            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = path,
                ["WaitForExist"] = false,
                ["TimeoutSeconds"] = 1.0
            };

            Assert.Throws<FileNotFoundException>(() => WorkflowInvoker.Invoke(new WaitFile(), inputs));
        }
    }
}