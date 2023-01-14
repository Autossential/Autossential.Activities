using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Autossential.Tests
{
    [TestClass]
    public class WaitFileTests
    {
        [ClassCleanup]
        public static void CleanUp()
        {
            IOSamples.ClearFolder();
        }

        [TestMethod]
        public void Default()
        {
            var path = IOSamples.CreateFile("output/sample.txt");
            var result = WorkflowTester.Run(new WaitFile(), GetArgs(path));
            var info = result.Get(p => p.Result) as FileInfo;
            Assert.AreEqual("sample.txt", info.Name);
        }

        [TestMethod]
        [DataRow(50)]
        [DataRow(30000)]
        public async Task WaitForExistAndInterval(int interval)
        {
            var createFile = CreateFileAfter(3, ".json");
            var result = WorkflowTester.Run(new WaitFile
            {
                WaitForExist = true,
                Interval = interval,
            }, GetArgs(IOSamples.GetTestPath("output/sample.json")));
            var expectedFileName = await createFile.ConfigureAwait(false);
            var info = result.Get(p => p.Result) as FileInfo;
            Assert.AreEqual("sample.json", info.Name);
        }

        [TestMethod]
        [DataRow(3000)]
        public void LockedFile(int unlockAfterMs)
        {
            var path = IOSamples.CreateFile("output/sample.txt");
            var fs = File.Open(path, FileMode.Append, FileAccess.Write);

            if (!IsLocked(path))
                Assert.Fail("File is not locked");

            Task.Delay(unlockAfterMs).ContinueWith(_ =>
            {
                fs.Dispose();

                if (IsLocked(path))
                    Assert.Fail("File is not unlocked");
            });

            WorkflowTester.Run(new WaitFile(), GetArgs(path));

            Assert.IsFalse(IsLocked(path));
        }

        private static bool IsLocked(string path)
        {
            try
            {
                using (var fs2 = File.OpenRead(path)) { }
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }


        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Timeout(bool waitForExist)
        {
            IOSamples.CreateFolder("output");

            object exec() => WorkflowTester.Run(new WaitFile()
            {
                Timeout = 2000,
                WaitForExist = waitForExist,

            }, GetArgs(IOSamples.GetTestPath("output/missing.txt")));

            if (waitForExist)
                Assert.ThrowsException<TimeoutException>(exec);
            else
                Assert.ThrowsException<FileNotFoundException>(exec);
        }

        private static IDictionary<string, object> GetArgs(string filePath)
        {
            return new Dictionary<string, object>
            {
                { nameof(WaitFile.FilePath), filePath }
            };
        }

        private static async Task<string> CreateFileAfter(int seconds, string extension = ".txt")
        {
            await Task.Delay(seconds * 1000).ConfigureAwait(false);
            return IOSamples.CreateFile("output/sample" + extension);
        }
    }
}
