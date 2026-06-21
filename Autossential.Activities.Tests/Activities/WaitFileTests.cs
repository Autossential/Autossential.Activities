using System.Activities;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class WaitFileTests : BaseTests
    {
        [Test]
        public async Task WhenFileAccessible_ReturnsFileInfo()
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
                var info = await Assert.That(result["Result"]).IsTypeOf<FileInfo>();
                await Assert.That(info.FullName).IsEqualTo(Path.GetFullPath(path));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        [Arguments(false)]
        [Arguments(true)]
        public async Task WhenFileLocked_TimesOut(bool continueOnError)
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
                    ["PollingIntervalSeconds"] = 0.5,
                    ["ContinueOnError"] = continueOnError
                };

                if (!continueOnError)
                    await Assert.That(() => WorkflowInvoker.Invoke(new WaitFile(), inputs))
                        .Throws<TimeoutException>();
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public async Task WhenFilePathIsNull_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = null!,
                ["TimeoutSeconds"] = 1.0
            };

            // The activity throws NullReferenceException when the FilePath argument is null
            await Assert.That(() => WorkflowInvoker.Invoke(new WaitFile(), inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        public async Task DynamicFile_WhenFilePresent_ReturnsFileInfo()
        {
            var dir = NewDir();

            var file = Path.Combine(dir, "test.txt");
            File.WriteAllText(file, "content");

            var inputs = new Dictionary<string, object>
            {
                ["DirectoryPath"] = dir,
                ["SearchPattern"] = "test.txt",
                ["TimeoutSeconds"] = 50.0,
                ["PollingIntervalSeconds"] = 0.1
            };

            var result = WorkflowInvoker.Invoke(new WaitFile { DynamicFile = true }, inputs);
            var info = await Assert.That(result["Result"]).IsTypeOf<FileInfo>();
            await Assert.That(info.FullName).IsEqualTo(Path.GetFullPath(file));
        }

        [Test]
        public async Task DynamicFile_WhenDirectoryPathIsNull_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DirectoryPath"] = null!,
                ["TimeoutSeconds"] = 1.0
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new WaitFile { DynamicFile = true }, inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        public async Task WhenWaitForExistFalse_FileDoesNotExist_ThrowsFileNotFoundException()
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

            await Assert.That(() => WorkflowInvoker.Invoke(new WaitFile(), inputs))
                .Throws<FileNotFoundException>();
        }
    }
}