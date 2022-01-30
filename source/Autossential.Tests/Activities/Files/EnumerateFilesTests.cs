using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.IO;
using System.Linq;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class EnumerateFilesTests
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
            IOSamples.CreateFolderAndFiles("output", "T1.txt", "Y1.yml", "J1.json");
            IOSamples.CreateFolderAndFiles("output/inner1", "T2.txt");
            IOSamples.CreateFolderAndFiles("output/inner2", "Y2.yml");
            IOSamples.CreateFolderAndFiles("output/inner3/deeper", "J2.json");
            IOSamples.CreateFolderAndFiles("output/inner3/deeper", "T3.yml");
        }

        [TestMethod]
        [DataRow(null, SearchOption.TopDirectoryOnly)]
        [DataRow(null, SearchOption.AllDirectories)]
        [DataRow("*.json", SearchOption.AllDirectories)]
        [DataRow("*.yml", SearchOption.AllDirectories)]
        [DataRow("*.txt", SearchOption.AllDirectories)]
        public void Default(string searchPattern, SearchOption option)
        {
            var path = IOSamples.GetTestPath("output");

            var enumFiles = Directory.EnumerateFiles(path, searchPattern ?? "*", option);
            var result = WorkflowTester.CompileAndInvoke(new EnumerateFiles()
            {
                DirectoryPath = new InArgument<string>(path),
                SearchPattern = new InArgument<string>(searchPattern),
                SearchOption = option
            });

            CollectionAssert.AreEqual(enumFiles.ToList(), result.ToList());
        }

        [TestMethod]
        [DataRow(FileAttributes.Hidden, FileAttributes.Hidden, 4, 3)]
        [DataRow(FileAttributes.Hidden | FileAttributes.Temporary, FileAttributes.Temporary, 1, 6)]
        [DataRow(FileAttributes.Normal, FileAttributes.Archive, 0, 0)]
        public void Attributes(FileAttributes attrToSet, FileAttributes attrToExclude, int filesToAffect, int expectedCount)
        {
            var path = IOSamples.GetTestPath("output");

            var filesToChange = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                .OrderBy(_ => Guid.NewGuid())
                .Take(filesToAffect)
                .Select(p => new FileInfo(p)).ToArray();

            foreach (var f in filesToChange)
                f.Attributes = attrToSet;

            var result = WorkflowTester.CompileAndInvoke(new EnumerateFiles()
            {
                DirectoryPath = new InArgument<string>(IOSamples.GetTestPath("output")),
                SearchOption = SearchOption.AllDirectories,
                Exclusions = attrToExclude
            });

            Assert.AreEqual(expectedCount, result.Count());
        }

        [TestMethod]
        public void MultiplePathsAndPatterns()
        {
            var path1 = IOSamples.GetTestPath("output");
            var path2 = IOSamples.GetTestPath("output/inner3/deeper");

            var result = WorkflowTester.CompileAndInvoke(new EnumerateFiles()
            {
                DirectoryPath = new InArgument<string[]>(_ => new[] { path1, path2 }),
                SearchPattern = new InArgument<string[]>(_ => new[] { "*.json", "*.txt" }),
                SearchOption = SearchOption.TopDirectoryOnly
            });

            var path1Res = Directory.EnumerateFiles(path1, "*.*", SearchOption.TopDirectoryOnly).Where(path => Path.GetExtension(path) != ".yml");
            var path2Res = Directory.EnumerateFiles(path2, "*.json", SearchOption.TopDirectoryOnly);

            CollectionAssert.AreEqual(path1Res.Concat(path2Res).ToArray(), result.ToArray());
        }

        [TestMethod]
        public void InvalidArgs()
        {
            Assert.ThrowsException<InvalidWorkflowException>(() => WorkflowTester.CompileAndInvoke(new EnumerateFiles()
            {
                DirectoryPath = new InArgument<int>(10),
                SearchPattern = new InArgument<bool>(true)
            }));

            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.CompileAndInvoke(new EnumerateFiles()));
        }
    }
}
