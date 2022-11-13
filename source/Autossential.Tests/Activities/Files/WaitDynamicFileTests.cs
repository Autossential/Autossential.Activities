using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class WaitDynamicFileTests
    {
        [ClassCleanup]
        public static void CleanUp()
        {
            IOSamples.ClearFolder();
        }

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            IOSamples.CreateFolder("output");
        }

        [TestMethod]
        public async Task Default()
        {
            var createFile = CreateFileAfter(1);
            var result = WorkflowTester.Run(new WaitDynamicFile(), GetArgs(IOSamples.GetTestPath("output"), null));
            var expectedFileName = await createFile.ConfigureAwait(false);
            var info = result.Get(p => p.Result) as FileInfo;
            Assert.AreEqual(Path.GetFullPath(expectedFileName), info.FullName);
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task FromDateTime(bool setDate)
        {
            DateTime? fromDate = null;
            if (setDate)
                fromDate = DateTime.Now;

            var createFile = CreateFileAfter(1);
            await Task.Delay(2000);
            var args = GetArgs(IOSamples.GetTestPath("output"), "*.txt");
            args.Add(nameof(WaitDynamicFile.Timeout), 2000);
            args.Add(nameof(WaitDynamicFile.ContinueOnError), true);
            args.Add(nameof(WaitDynamicFile.FromDateTime), fromDate);

            var result = WorkflowTester.Run(new WaitDynamicFile(), args);
            var expectedFileName = await createFile.ConfigureAwait(false);
            var info = result.Get(p => p.Result) as FileInfo;
            if (setDate)
            {
                Assert.AreEqual(Path.GetFullPath(expectedFileName), info.FullName);
            }
            else
            {
                Assert.IsNull(info);
            }            
        }

        [TestMethod]
        public async Task SearchPattern()
        {
            _ = CreateFileAfter(1, ".json");
            _ = CreateFileAfter(2, ".yml");
            var createFile = CreateFileAfter(3, ".txt");
            var result = WorkflowTester.Run(new WaitDynamicFile(), GetArgs(IOSamples.GetTestPath("output"), "*.txt"));
            var expectedFileName = await createFile.ConfigureAwait(false);
            var info = result.Get(p => p.Result) as FileInfo;
            Assert.AreEqual(Path.GetFullPath(expectedFileName), info.FullName);
        }

        [TestMethod]
        [DataRow(1, 50)] // clamps 100
        [DataRow(1, 30000)] // clamps 20000
        public async Task Intervals(int secondsToCreateFile, int interval)
        {
            var createFile = CreateFileAfter(secondsToCreateFile, ".txt");
            var result = WorkflowTester.Run(new WaitDynamicFile
            {
                Interval = interval
            }, GetArgs(IOSamples.GetTestPath("output"), "*.txt"));
            var expectedFileName = await createFile.ConfigureAwait(false);
            var info = result.Get(p => p.Result) as FileInfo;
            Assert.AreEqual(Path.GetFullPath(expectedFileName), info.FullName);
        }

        [TestMethod]
        public void Timeout()
        {
            Assert.ThrowsException<TimeoutException>(() => WorkflowTester.Run(new WaitDynamicFile()
            {
                Timeout = 2000
            }, GetArgs(IOSamples.GetTestPath("output"), "*.txt")));
        }

        private static IDictionary<string, object> GetArgs(string directoryPath, string searchPattern)
        {
            return new Dictionary<string, object>
            {
                { nameof(WaitDynamicFile.DirectoryPath), directoryPath },
                { nameof(WaitDynamicFile.SearchPattern), searchPattern }
            };
        }

        private static async Task<string> CreateFileAfter(int seconds, string extension = ".txt")
        {
            await Task.Delay(seconds * 1000).ConfigureAwait(false);
            return IOSamples.CreateFile("output/" + Guid.NewGuid().ToString("n") + extension);
        }
    }
}
