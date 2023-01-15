using Autossential.Activities;
using Autossential.Core.Models;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Autossential.Tests
{
    [TestClass]
    public class CleanUpFolderTests
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

            IOSamples.CreateFiles("T1.txt", "Y1.yml", "J1.json");
            IOSamples.CreateFolderAndFiles("inner1", "T2.txt");

            // empty folder
            IOSamples.CreateFolder("inner2");

            IOSamples.CreateFolderAndFiles("inner3", "Y2.yml");
            IOSamples.CreateFolderAndFiles("inner3/deeper", "J2.json", "T3.txt");
        }

        public static void ChangeLastWriteTime()
        {
            File.SetLastWriteTime(IOSamples.GetTestPath("T1.txt"), DateTime.Now.AddMinutes(-1));
            File.SetLastWriteTime(IOSamples.GetTestPath("Y1.yml"), DateTime.Now.AddMinutes(-3));
            File.SetLastWriteTime(IOSamples.GetTestPath("J1.json"), DateTime.Now.AddMinutes(-3));
            File.SetLastWriteTime(IOSamples.GetTestPath("inner1/T2.txt"), DateTime.Now.AddMinutes(-10));
            File.SetLastWriteTime(IOSamples.GetTestPath("inner3/deeper/J2.json"), DateTime.Now.AddMinutes(-2));
            File.SetLastWriteTime(IOSamples.GetTestPath("inner3/deeper/T3.txt"), DateTime.Now.AddMinutes(-2));
        }

        [TestMethod]
        [DataRow(true, null, 7, 4)]
        [DataRow(false, null, 7, 0)]
        [DataRow(true, "*", 7, 4)]
        [DataRow(false, "*.*", 7, 0)]
        [DataRow(true, "*.yml", 2, 1)]
        [DataRow(true, "*.json,*.txt", 5, 3)]
        [DataRow(false, "*.exe", 0, 0)]
        [DataRow(true, "*.exe", 0, 1)]
        public void Default(bool deleteEmptyFolders, object searchPattern, int filesDeleted, int foldersDeleted)
        {
            InArgument pattern = null;
            if (searchPattern != null)
            {
                var value = searchPattern.ToString();
                if (value.Contains(','))
                {
                    var values = value.Split(',');
                    pattern = new InArgument<string[]>(_ => values);
                }
                else
                {
                    pattern = new InArgument<string>(value);
                }
            }

            var folder = IOSamples.GetTestPath();
            var output = WorkflowTester.CompileAndRun(new CleanUpFolder
            {
                DeleteEmptyFolders = deleteEmptyFolders,
                SearchPattern = pattern
            }, GetArgs(folder, null));

            var result = (CleanUpFolderResult)output.Get(p => p.Result);
            Assert.AreEqual(filesDeleted, result.FilesDeleted);
            Assert.AreEqual(foldersDeleted, result.FoldersDeleted);
            Assert.AreEqual(filesDeleted + foldersDeleted, result.TotalDeleted);
        }

        [TestMethod]
        [DataRow(true, 0, 7, 4)]
        [DataRow(false, 0, 7, 0)]
        [DataRow(true, -7, 1, 2)]
        [DataRow(true, -5, 1, 2)]
        [DataRow(true, -2, 5, 3)]
        [DataRow(true, -1, 6, 3)]
        public void LastWriteTime(bool deleteEmptyFolders, int minutesToAdd, int filesDeleted, int foldersDeleted)
        {
            ChangeLastWriteTime();

            var folder = IOSamples.GetTestPath();
            var output = WorkflowTester.CompileAndRun(new CleanUpFolder
            {
                DeleteEmptyFolders = deleteEmptyFolders
            }, GetArgs(folder, DateTime.Now.AddMinutes(minutesToAdd)));

            var result = (CleanUpFolderResult)output.Get(p => p.Result);
            Assert.AreEqual(filesDeleted, result.FilesDeleted);
            Assert.AreEqual(foldersDeleted, result.FoldersDeleted);
            Assert.AreEqual(filesDeleted + foldersDeleted, result.TotalDeleted);
        }

        [TestMethod]
        public void InvalidSearchPattern()
        {
            Assert.ThrowsException<InvalidWorkflowException>(() =>
                WorkflowTester.CompileAndRun(new CleanUpFolder
                {
                    SearchPattern = new InArgument<int>(100)
                }, GetArgs(IOSamples.GetTestPath(), null)));
        }

        private static Dictionary<string, object> GetArgs(string folder, DateTime? lastWriteTime)
        {
            var dic = new Dictionary<string, object>
            {
                { nameof(CleanUpFolder.Folder), folder }
            };

            if (lastWriteTime != null)
                dic.Add(nameof(CleanUpFolder.LastWriteTime), lastWriteTime);

            return dic;
        }
    }
}
