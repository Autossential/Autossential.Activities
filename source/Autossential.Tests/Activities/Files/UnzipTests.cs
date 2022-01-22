using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class UnzipTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            IOSamples.ClearFolder();
        }

        [TestInitialize]
        public void Initialize()
        {
            Cleanup();

            // excel.svg, hello.txt, link.png
            IOSamples.ExportSample("files1.zip");

            // hello.txt
            // folder1/(excel.svg, hello.txt, link.png)
            // folder1/folder2/link.png
            IOSamples.ExportSample("files2.zip");

        }

        [TestMethod]
        [DataRow("files1.zip", 3, 0)]
        [DataRow("files2.zip", 5, 2)]
        public void Default(string sampleFile, int expectedFilesCount, int expectedFoldersCount)
        {
            var extractTo = IOSamples.GetTestPath("unzip");
            WorkflowTester.Run(new Unzip(), GetArgs(IOSamples.GetTestPath(sampleFile), extractTo));
            var files = Directory.GetFiles(extractTo, "*", SearchOption.AllDirectories);
            var folders = Directory.GetDirectories(extractTo, "*", SearchOption.AllDirectories);
            Assert.AreEqual(expectedFilesCount, files.Length);
            Assert.AreEqual(expectedFoldersCount, folders.Length);
        }

        [TestMethod]

        public void Override()
        {
            var files1 = IOSamples.GetTestPath("files1.zip");
            var files2 = IOSamples.GetTestPath("files2.zip");
            var extractTo = IOSamples.GetTestPath("unzip");

            WorkflowTester.Run(new Unzip(), GetArgs(files1, extractTo));

            Assert.ThrowsException<IOException>(() => WorkflowTester.Run(new Unzip { Overwrite = false }, GetArgs(files2, extractTo)));

            // no error
            WorkflowTester.Run(new Unzip { Overwrite = true }, GetArgs(files2, extractTo));
        }

        private static IDictionary<string, object> GetArgs(object zipFilePath, object extractTo)
        {
            return new Dictionary<string, object>
            {
                { nameof(Unzip.ZipFilePath), zipFilePath },
                { nameof(Unzip.ExtractTo), extractTo }
            };
        }
    }
}
