using Autossential.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;

namespace Autossential.Tests
{
    [TestClass]
    public class CheckPointTests
    {
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Default(bool expression)
        {
            try
            {
                WorkflowInvoker.Invoke(new CheckPoint(), GetArgs(expression, new Exception("The expression is false")));
                Assert.IsTrue(expression);
            }
            catch (Exception e)
            {
                Assert.IsFalse(expression);
                Assert.AreEqual("The expression is false", e.Message);
            }
        }

        [TestMethod]
        public void EmptyArgs()
        {
            Assert.ThrowsException<ArgumentException>(() => WorkflowInvoker.Invoke(new CheckPoint()));
        }

        [TestMethod]
        public void WithData()
        {
            try
            {
                var activity = new CheckPoint();
                activity.Data.Add("Name", new InArgument<string>("SomeName"));
                activity.Data.Add("Length", new InArgument<int>(10));

                WorkflowInvoker.Invoke(activity, GetArgs(false, new Exception()));
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Data.Count > 0);
                Assert.AreEqual(e.Data["Name"], "SomeName");
                Assert.AreEqual(e.Data["Length"], 10);
            }
        }

        private static Dictionary<string, object> GetArgs(object expression, object exception)
        {
            return new Dictionary<string, object>
            {
                { nameof(CheckPoint.Expression), expression },
                { nameof(CheckPoint.Exception), exception }
            };
        }
    }
}
