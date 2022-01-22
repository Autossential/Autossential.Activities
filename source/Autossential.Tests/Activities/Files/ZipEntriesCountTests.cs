using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class ZipEntriesCountTests
    {
        [TestMethod]
        [DataRow("files1.zip", 3, 0)]
        [DataRow("files2.zip", 5, 2)]
        public void Default(string fileName, int files, int folders)
        {
            int filesCount = 0;
            var result = WorkflowTester.Run(new ZipEntriesCount
            {
                FilesCount = new System.Activities.OutArgument<int>(_ => filesCount)
            }, GetArgs(IOSamples.GetSamplePath(fileName)));

            Assert.AreEqual(files, filesCount);
            Assert.AreEqual(files, result.Get(p => p.FilesCount));
            Assert.AreEqual(folders, result.Get(p => p.FoldersCount));
            Assert.AreEqual(files + folders, result.Get(p => p.EntriesCount));
        }

        [TestMethod]
        public void InvalidPath()
        {
            int filesCount = 0;
            Assert.ThrowsException<FileNotFoundException>(() => WorkflowTester.Run(new ZipEntriesCount()
            {
                FilesCount = new System.Activities.OutArgument<int>(_ => filesCount)
            }, GetArgs("../fake.zip")));
        }


        private static Dictionary<string, object> GetArgs(string zipFilePath)
        {
            return new Dictionary<string, object>
            {
                { nameof(ZipEntriesCount.ZipFilePath), zipFilePath }
            };
        }
    }
}
