using Autossential.Activities.Tests.Extensions;
using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class MapUnmapTests
    {
        private const string NetworkPath = @"\\127.0.0.1\Shared";
      
        private static string[] GetLogicalDrives() => Environment.GetLogicalDrives();

        [Fact]
        public void Execute_ShouldMapAndUnmap_CustomDriveLetter()
        {
            var map = new MapDrive
            {
                SharedDrivePath = new InArgument<string>(NetworkPath)
            };

            var drivers = GetLogicalDrives();
            var driversCount = drivers.Length;

            var (result, outputs) = WorkflowInvoker.InvokeOutputs(map);
            Assert.True(result);
            Assert.False(string.IsNullOrEmpty(outputs[nameof(MapDrive.MappedDrive)].ToString()));

            var newDrivers = GetLogicalDrives().Except(drivers).ToArray();
            Assert.Single(newDrivers);

            var unmap = new UnmapDrive
            {
                DriveLetter = newDrivers[0].TrimEnd('\\')
            };

            Assert.True(WorkflowInvoker.Invoke(unmap));
            Assert.Equal(driversCount, GetLogicalDrives().Length);
        }

        [Fact]
        public void Execute_MapDrive_MissingSharedDrivePath_Throws()
        {
            // SharedDrivePath is required; validation occurs before execution and raises ArgumentException
            Assert.Throws<ArgumentException>(() => WorkflowInvoker.Invoke(new MapDrive()));
        }

        [Fact]
        public void Execute_UnmapDrive_MissingDriveLetter_Throws()
        {
            // DriveLetter is required for unmapping; workflow validation raises ArgumentException when missing
            Assert.Throws<ArgumentException>(() => WorkflowInvoker.Invoke(new UnmapDrive()));
        }

        [Fact]
        public void Execute_UnmapDrive_InvalidDriveLetterFormat_Throws()
        {
            // Invalid drive letter formats (not like 'X:') should be rejected
            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new UnmapDrive(), new Dictionary<string, object>
            {
                { nameof(UnmapDrive.DriveLetter), "XYZ" }
            }));
        }

        [Fact]
        public void Execute_MapDrive_InvalidDriveLetterFormat_Throws()
        {
            // MapDrive should validate provided drive letter format and throw when invalid
            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new MapDrive(), new Dictionary<string, object>
            {
                { nameof(MapDrive.DriveLetter), "A" },
                { nameof(MapDrive.SharedDrivePath), NetworkPath }
            }));
        }

        [Fact]
        public void Execute_MapDrive_SetsResponseCodeAndMessage()
        {
            // Ensure ResponseCode and ResponseMessage out arguments are populated when invoking
            var map = new MapDrive
            {
                SharedDrivePath = new InArgument<string>(NetworkPath)
            };

            var (result, outputs) = WorkflowInvoker.InvokeOutputs(map);

            // result indicates success of activity; outputs should contain response information
            Assert.NotNull(outputs);
            Assert.True(outputs.ContainsKey(nameof(MapDrive.ResponseCode)));
            Assert.True(outputs.ContainsKey(nameof(MapDrive.ResponseMessage)));

            // ResponseCode should be convertible to int and ResponseMessage a non-empty string
            var codeObj = outputs[nameof(MapDrive.ResponseCode)];
            var msgObj = outputs[nameof(MapDrive.ResponseMessage)];

            Assert.NotNull(codeObj);
            Assert.IsType<int>(codeObj);
            Assert.NotNull(msgObj);
            Assert.IsType<string>(msgObj);
        }

        [Fact]
        public void Execute_ShoudMapAndUnmap_SpecificDriveLetter()
        {
            const string driveLetter = "X:";
            var (result, outputs) = WorkflowInvoker.InvokeOutputs(new MapDrive(), new Dictionary<string, object>
            {
                { nameof(MapDrive.DriveLetter), driveLetter },
                { nameof(MapDrive.SharedDrivePath), NetworkPath }
            });

            Assert.True(result);
            Assert.Equal(driveLetter, outputs[nameof(MapDrive.MappedDrive)]);

            Assert.True(WorkflowInvoker.Invoke(new UnmapDrive(), new Dictionary<string, object>
            {
                { nameof(UnmapDrive.DriveLetter), driveLetter },
            }));
        }

        [Fact]
        public void Execute_SameLetterMapWithoutForce_ShouldReturnFalse()
        {
            const string driveLetter = "X:";
            var (result, outputs) = WorkflowInvoker.InvokeOutputs(new MapDrive(), new Dictionary<string, object>
            {
                { nameof(MapDrive.DriveLetter), driveLetter },
                { nameof(MapDrive.SharedDrivePath), NetworkPath }
            });

            Assert.True(result);
            Assert.Equal(driveLetter, outputs[nameof(MapDrive.MappedDrive)]);

            (result, _) = WorkflowInvoker.InvokeOutputs(new MapDrive(), new Dictionary<string, object>
            {
                { nameof(MapDrive.DriveLetter), driveLetter },
                { nameof(MapDrive.SharedDrivePath), NetworkPath }
            });

            Assert.False(result);
            Assert.True(WorkflowInvoker.Invoke(new UnmapDrive(), new Dictionary<string, object>
            {
                { nameof(UnmapDrive.DriveLetter), driveLetter },
            }));
        }


        [Fact]
        public void Execute_SameLetterMapWithForce_ShouldReturnTrue()
        {
            const string driveLetter = "X:";
            var (result, outputs) = WorkflowInvoker.InvokeOutputs(new MapDrive(), new Dictionary<string, object>
            {
                { nameof(MapDrive.DriveLetter), driveLetter },
                { nameof(MapDrive.SharedDrivePath), NetworkPath }
            });

            Assert.True(result);
            Assert.Equal(driveLetter, outputs[nameof(MapDrive.MappedDrive)]);

            (result, _) = WorkflowInvoker.InvokeOutputs(new MapDrive(), new Dictionary<string, object>
            {
                { nameof(MapDrive.DriveLetter), driveLetter },
                { nameof(MapDrive.SharedDrivePath), NetworkPath },
                { nameof(MapDrive.Force), true }
            });

            Assert.True(result);
            Assert.True(WorkflowInvoker.Invoke(new UnmapDrive(), new Dictionary<string, object>
            {
                { nameof(UnmapDrive.DriveLetter), driveLetter },
            }));
        }
    }
}