using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class TerminateProcessTests
    {
        [Fact]
        public void Invoke_WithValidProcessName_DoesNotThrow()
        {
            // Use a process that likely exists on the system but not critical to test
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = new List<string> { "nonexistent_process_xyz" }
            };

            // Should not throw - it gracefully handles non-existent processes
            WorkflowInvoker.Invoke(new TerminateProcess(), inputs);
        }

        [Fact]
        public void Invoke_WithEmptyProcessNameList_DoesNotThrow()
        {
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = new List<string> { }
            };

            WorkflowInvoker.Invoke(new TerminateProcess(), inputs);
        }

        [Fact]
        public void Invoke_WithMultipleProcessNames_DoesNotThrow()
        {
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = new List<string> { "notepad", "calc", "nonexistent" }
            };

            // Should not throw
            WorkflowInvoker.Invoke(new TerminateProcess(), inputs);
        }

        [Fact]
        public void Invoke_WithNullProcessNames_ThrowsNullReferenceException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = null!
            };

            Assert.Throws<NullReferenceException>(() => WorkflowInvoker.Invoke(new TerminateProcess(), inputs));
        }

        [Fact]
        public void Invoke_IntegrationTesting_ClosesAllProcesses()
        {
            Process.Start("notepad");
            Process.Start("calc");
            Thread.Sleep(1000); // waits for load
            Assert.True(Process.GetProcessesByName("notepad").Length > 0);
            Assert.True(Process.GetProcessesByName("CalculatorApp").Length > 0);
            WorkflowInvoker.Invoke(new TerminateProcess(), new Dictionary<string, object>
            {
                ["ProcessNames"] = new[] { "notepad", "CalculatorApp" }
            });
            Assert.True(Process.GetProcessesByName("notepad").Length == 0);
            Assert.True(Process.GetProcessesByName("CalculatorApp").Length == 0);
        }
    }
}
