using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class ZipTests
    {
        [TestCleanup]
        public void Clean()
        {
            IOSamples.ClearFolder();
        }

        [TestInitialize]
        public void Initialize()
        {
            IOSamples.ClearFolder();
            IOSamples.CreateFolderAndFiles("output", "1.txt", "2.yml", "3.json");
            IOSamples.CreateFolderAndFiles("output/A", "1.txt");
            IOSamples.CreateFolderAndFiles("output/B", "1.txt", "2.yml");
            IOSamples.CreateFolderAndFiles("output/C/D", "3.json");
            IOSamples.CreateFolder("output/C/E");
        }

        [TestMethod]
        public void SameRoot()
        {
            var zipFilePath = IOSamples.GetTestPath("output/result.zip");

            var sources = new[]
            {
                IOSamples.GetTestPath("output/A"),
                IOSamples.GetTestPath("output/B"),
                IOSamples.GetTestPath("output/C")
            };

            WorkflowTester.CompileAndRun(new Zip
            {
                ToCompress = new InArgument<string[]>(_ => sources),
            }, GetArgs(zipFilePath));

            using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read))
            {
                Assert.AreEqual(5, zip.Entries.Count);
                Assert.AreEqual(4, zip.Entries.Count(entry => !string.IsNullOrEmpty(entry.Name)));
            }
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public void DifferentRoot(bool shortNames)
        {
            var zipFilePath = IOSamples.GetTestPath("output/result.zip");

            var a = IOSamples.CreateFile("C:\\Temp\\T1\\a.txt");
            var b = Path.GetFullPath(IOSamples.CreateFile("output/temp/T1/b.txt")).Replace("\\", "/"); // D:
            var c = Path.GetFullPath(IOSamples.CreateFile("output/temp/T2/c.txt")).Replace("\\", "/"); // D:

            var sources = new[] { a, b, c };
            WorkflowTester.CompileAndRun(new Zip
            {
                ToCompress = new InArgument<string[]>(_ => sources),
                ShortEntryNames = shortNames
            }, GetArgs(zipFilePath));

            File.Delete(a);

            using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read))
            {
                if (shortNames)
                {
                    Assert.AreEqual("C\\a.txt", zip.Entries[0].FullName);
                    Assert.AreEqual("D\\T1\\b.txt", zip.Entries[1].FullName);
                    Assert.AreEqual("D\\T2\\c.txt", zip.Entries[2].FullName);
                }
                else
                {
                    Assert.AreEqual(Path.GetFullPath(a).Replace(":", ""), zip.Entries[0].FullName);
                    Assert.AreEqual(Path.GetFullPath(b).Replace(":", ""), zip.Entries[1].FullName);
                    Assert.AreEqual(Path.GetFullPath(c).Replace(":", ""), zip.Entries[2].FullName);
                }
            }
        }


        [TestMethod]
        public void Update()
        {
            var zipFilePath = IOSamples.GetTestPath("output/result.zip");
            WorkflowTester.CompileAndRun(new Zip
            {
                ToCompress = new InArgument<string>(IOSamples.GetTestPath("output/A")),
                CompressionLevel = CompressionLevel.NoCompression
            }, GetArgs(zipFilePath));

            WorkflowTester.CompileAndRun(new Zip
            {
                ToCompress = new InArgument<string>(IOSamples.GetTestPath("output/1.txt")),
                CompressionLevel = CompressionLevel.Fastest
            }, GetArgs(zipFilePath));

            WorkflowTester.CompileAndRun(new Zip
            {
                ToCompress = new InArgument<string>(IOSamples.GetTestPath("output/B/1.txt")),
                CompressionLevel = CompressionLevel.Optimal
            }, GetArgs(zipFilePath)); ;

            using (var zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Read))
            {
                Assert.AreEqual(1, zip.Entries.Count);
            }
        }


        private static IDictionary<string, object> GetArgs(object zipFilePath)
        {
            return new Dictionary<string, object>
            {
                { nameof(Zip.ZipFilePath), zipFilePath }
            };
        }
    }
}
