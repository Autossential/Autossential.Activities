using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Diagnostics;

namespace Autossential.Tests
{

    [TestClass]
    public class TerminateProcessTests
    {
        [TestMethod]
        public void ManualTest()
        {
            var processes = new[] {
                "notepad",
                "notepad++",
                "DummyNotResponding",
                "MSEDGE"
            };

            WorkflowTester.Invoke(new TerminateProcess()
            {
                ProcessName = new InArgument<string[]>(_ => processes),
                Timeout = new InArgument<int>(_ => 700_000)
            });

            foreach (var name in processes)
            {
                Assert.AreEqual(0, Process.GetProcessesByName(name).Length);
            }
        }
    }
}
