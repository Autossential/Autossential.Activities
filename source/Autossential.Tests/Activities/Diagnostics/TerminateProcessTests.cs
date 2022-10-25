using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Diagnostics;
using System.Threading;

namespace Autossential.Activities.Test
{

    [TestClass]
    public class TerminateProcessTests
    {
        [TestMethod]
        public void Notepad()
        {
            for (int i = 0; i < 2; i++)
            {
                Process.Start("notepad");
                Process.Start("calc");
                Thread.Sleep(200);
            }

            var processes = new[] { "notepad", "Calculator" };
            WorkflowTester.Invoke(new TerminateProcess()
            {
                ProcessName = new InArgument<string[]>(_ => processes)
            });

            foreach (var name in processes)
            {
                Assert.AreEqual(0, Process.GetProcessesByName(name).Length);
            }
        }

        [TestMethod]
        public void Edge()
        {
            var processes = Process.GetProcessesByName("msedge");

            if (processes.Length > 0)
            {
                WorkflowTester.Invoke(new TerminateProcess
                {
                    ProcessName = new InArgument<string>("msedge")
                });

                Assert.AreEqual(0, Process.GetProcessesByName("msedge").Length);
            }
        }
    }
}
