using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    [NotInParallel]
    public class TerminateProcessTests
    {
        [Test]
        public void WithValidProcessName_DoesNotThrow()
        {
            // Use a process that likely exists on the system but not critical to test
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = new List<string> { "nonexistent_process_xyz" }
            };

            // Should not throw - it gracefully handles non-existent processes
            WorkflowInvoker.Invoke(new TerminateProcess(), inputs);
        }

        [Test]
        public void WithEmptyProcessNameList_DoesNotThrow()
        {
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = new List<string> { }
            };

            WorkflowInvoker.Invoke(new TerminateProcess(), inputs);
        }

        [Test]
        public void WithMultipleProcessNames_DoesNotThrow()
        {
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = new List<string> { "notepad", "calc", "nonexistent" }
            };

            // Should not throw
            WorkflowInvoker.Invoke(new TerminateProcess(), inputs);
        }

        [Test]
        public async Task WithNullProcessNames_ThrowsNullReferenceException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["ProcessNames"] = null!
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new TerminateProcess(), inputs))
                .Throws<NullReferenceException>();
        }

        [Test]
        public async Task IntegrationTesting_ClosesAllProcesses()
        {
            Process.Start("notepad");
            Process.Start("calc");
            Thread.Sleep(1000); // waits for load

            await Assert.That(Process.GetProcessesByName("notepad").Length > 0).IsTrue();
            await Assert.That(Process.GetProcessesByName("CalculatorApp").Length > 0).IsTrue();

            WorkflowInvoker.Invoke(new TerminateProcess(), new Dictionary<string, object>
            {
                ["ProcessNames"] = new[] { "notepad", "CalculatorApp" }
            });

            await Assert.That(Process.GetProcessesByName("notepad").Length == 0).IsTrue();
            await Assert.That(Process.GetProcessesByName("CalculatorApp").Length == 0).IsTrue();
        }
    }
}