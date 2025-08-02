using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Autossential.Tests
{

    [TestClass]
    public class MapAndUnmapDriveTests
    {
        private const string SharedPath = @"\\WINDEV2112EVAL\Shared";

        [TestMethod]
        [DataRow("   ")]
        [DataRow("9")]
        [DataRow("|")]
        [DataRow("ABC")]
        [DataRow("X*")]
        public void InvalidDriveLettersTest_Map(string letter)
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                WorkflowTester.Invoke(new MapDrive()
                {
                    DriveLetter = letter,
                    SharedDrivePath = @"\\hostname\shared"
                });
            });
        }

        [TestMethod]
        [DataRow("   ")]
        [DataRow("9")]
        [DataRow("|")]
        [DataRow("ABC")]
        [DataRow("X*")]
        public void InvalidDriveLettersTest_Unmap(string letter)
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                WorkflowTester.Invoke(new UnmapDrive()
                {
                    DriveLetter = letter
                });
            });
        }

        [TestMethod]
        [DataRow("A:")]
        [DataRow("B:\\")]
        [DataRow(null)]
        public void MapWithoutCredentials(string driveLetter)
        {
            var result = WorkflowTester.Run(new MapDrive(), GetArgsWithoutCredentials(driveLetter));
            driveLetter = (string)result.Get(p => p.MappedDrive);
            Assert.IsTrue((bool)result.Get(p => p.Result), "Check VirtualBox connection");

            if (driveLetter != null)
            {
                Assert.IsTrue(Environment.GetLogicalDrives().Contains(driveLetter));
            }
        }

        [TestMethod]
        [DataRow("A:")]
        [DataRow("B:")]
        [DataRow(null)]
        public void MapWithCredentials(string driveLetter)
        {
            var result = WorkflowTester.Invoke(new MapDrive(), GetArgsWithCredentials(driveLetter, "user", false));

            Assert.IsTrue(result, "Check VirtualBox connection");
            if (driveLetter != null)
            {
                Assert.IsTrue(Environment.GetLogicalDrives().Contains(driveLetter + "\\"));
            }
        }

        [TestMethod]
        public void MapWithDifferentUsers_ErrorCode_1219()
        {
            var result = WorkflowTester.Run(new MapDrive(), GetArgsWithCredentials("A", "user", false));
            var code = result.Get(p => p.ResponseCode);
            Assert.AreEqual(0, code);

            result = WorkflowTester.Run(new MapDrive(), GetArgsWithCredentials("B", "temp", false));
            code = result.Get(p => p.ResponseCode);
            Assert.AreEqual(1219, code);
        }

        [TestMethod]
        public void MapWithDifferentUsers_ForceMode_NoError()
        {
            var result = WorkflowTester.Run(new MapDrive(), GetArgsWithCredentials("A", "user", true));
            var code = result.Get(p => p.ResponseCode);
            Assert.AreEqual(0, code);

            result = WorkflowTester.Run(new MapDrive(), GetArgsWithCredentials("B", "temp", true));
            code = result.Get(p => p.ResponseCode);
            Assert.AreEqual(0, code);
        }

        [TestMethod]
        [DataRow("A:", false, 0)]
        [DataRow("A:", false, 1)]
        [DataRow("A:", true, 2)]
        public void MapWithCredentialsAndForceOption(string driveLetter, bool force, int index)
        {
            var result = WorkflowTester.Invoke(new MapDrive(), GetArgsWithCredentials(driveLetter, "user", force));
            if (index == 1)
            {
                Assert.IsFalse(result);
            }
            else
            {
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void UnmapDrivers()
        {
            var allResults = new List<bool>();
            var drivers = Environment.GetLogicalDrives();
            if (drivers.Length > 2)
            {
                foreach (var driver in drivers)
                {
                    if (driver[0] == 'C' || driver[0] == 'D')
                        continue;

                    var result = WorkflowTester.Invoke(new UnmapDrive(), GetArgsForUnmap(driver[0].ToString()));
                    allResults.Add(result);
                }

                Assert.IsTrue(allResults.All(p => p));
                Assert.AreEqual(Environment.GetLogicalDrives().Length, 2);
            }
            else
            {
                Assert.Inconclusive();
            }
        }

        [TestMethod]

        public void UnmapDriveWithInvalidParam()
        {
            var result = WorkflowTester.Invoke(new UnmapDrive(), GetArgsForUnmap("X"));
            Assert.IsTrue(result);
        }

        private static Dictionary<string, object> GetArgsForUnmap(string driveLetter)
        {
            return new Dictionary<string, object>
            {
                { nameof(UnmapDrive.DriveLetter), driveLetter }
            };
        }
        private static Dictionary<string, object> GetArgsWithoutCredentials(string driveLetter)
        {
            return new Dictionary<string, object>
            {
                { nameof(MapDrive.SharedDrivePath), SharedPath },
                { nameof(MapDrive.DriveLetter), driveLetter }
            };
        }

        private static Dictionary<string, object> GetArgsWithCredentials(string driveLetter, string user, bool force)
        {
            return new Dictionary<string, object>
            {
                { nameof(MapDrive.SharedDrivePath), SharedPath },
                { nameof(MapDrive.DriveLetter), driveLetter },
                { nameof(MapDrive.Force), force },
                { nameof(MapDrive.Credential), new NetworkCredential(user, "dev123", "windev2112eval") }
            };
        }
    }
}
