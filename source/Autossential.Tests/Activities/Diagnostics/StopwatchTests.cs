using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autossential.Tests
{
    [TestClass]
    public class StopwatchTests
    {
        [TestMethod]
        public void Initialize()
        {
            Stopwatch sw = null;
            var activity = new Autossential.Activities.Stopwatch()
            {
                ReferenceStopwatch = new System.Activities.InOutArgument<Stopwatch>(_ => sw)
            };

            WorkflowTester.Invoke(activity);
            Assert.IsNotNull(sw);
        }
    }
}
