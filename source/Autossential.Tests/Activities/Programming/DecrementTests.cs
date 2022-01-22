using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class DecrementTests
    {
        [TestMethod]
        [DataRow(9, 4)]
        [DataRow(8, 7)]
        [DataRow(5, 15)]
        public void Test(int start, int increment)
        {
            var result = WorkflowTester.Run(new Decrement(), GetArgs(start, increment));
            Assert.AreEqual(start - increment, result.Get(p => p.Variable));
        }

        [TestMethod]
        public void NegativeValue()
        {
            Assert.ThrowsException<InvalidOperationException>(() => WorkflowTester.Run(new Decrement(), GetArgs(10, -1)));
        }

        [TestMethod]
        [DataRow(10, null)]
        [DataRow(null, 10)]
        public void NullArgs(object variable, object value)
        {
            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.Run(new Decrement(), GetArgs(variable, value)));
        }

        private static Dictionary<string, object> GetArgs(object variable, object value)
        {
            return new Dictionary<string, object>
            {
                { nameof(Increment.Variable), variable },
                { nameof(Increment.Value), value }
            };
        }
    }
}
