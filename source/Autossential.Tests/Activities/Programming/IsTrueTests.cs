using Autossential.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;

namespace Autossential.Tests
{
    [TestClass]
    public class IsTrueTests
    {
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Default(bool expression)
        {
            var workflow = new IsTrue();
            var result = WorkflowInvoker.Invoke(workflow, GetArgs(expression));
            Assert.AreEqual(expression, result);
        }

        [TestMethod]
        public void NullArg()
        {
            var activity = new IsTrue();
            Assert.ThrowsException<ArgumentException>(() => WorkflowInvoker.Invoke(activity, GetArgs(null)));
        }

        [TestMethod]
        public void EmptyArgs()
        {
            var activity = new IsTrue();
            Assert.ThrowsException<ArgumentException>(() => WorkflowInvoker.Invoke(activity));
        }

        private static Dictionary<string, object> GetArgs(object value)
        {
            return new Dictionary<string, object>
            {
                { nameof(IsTrue.Value), value }
            };
        }
    }
}