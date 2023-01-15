using Autossential.Activities.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;

namespace Autossential.Tests
{
    [TestClass]
    public class TimeLoopTests
    {
        [TestMethod]
        public void Default()
        {
            var dyn = new DynamicActivity<int>();
            dyn.Implementation = () => new TimeLoop
            {
                Timer = new InArgument<TimeSpan>(TimeSpan.FromMilliseconds(100)),
                Body = new ActivityAction
                {
                    Handler = new Assign<int>
                    {
                        To = new OutArgument<int>(env => dyn.Result.Get(env)),
                        Value = new InArgument<int>(env => dyn.Result.Get(env) + 1)
                    }
                }
            };

            var result = WorkflowInvoker.Invoke(dyn);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void ExitOnError(bool exitOnException)
        {
            int max = 30;
            var dyn = new DynamicActivity<int>();
            dyn.Implementation = () => new TimeLoop
            {
                Timer = new InArgument<TimeSpan>(TimeSpan.FromMilliseconds(100)),
                ExitOnException = exitOnException,
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities =
                        {
                            new Assign<int>
                            {
                                To = new OutArgument<int>(env => dyn.Result.Get(env)),
                                Value = new InArgument<int>(env => dyn.Result.Get(env) + 1)
                            },
                            new If
                            {
                                Condition = new GreaterThan<int,int, bool>
                                {
                                    Left = new InArgument<int>(env=>dyn.Result.Get(env)),
                                    Right = new InArgument<int>(max)
                                },
                                Then = new Throw
                                {
                                    Exception = new InArgument<Exception>(_ => new Exception("Value greater than expected"))
                                }
                            }
                        }
                    }
                }
            };

            var result = WorkflowInvoker.Invoke(dyn);
            if (exitOnException)
            {
                Assert.AreEqual(result, max + 1);
            }
            else
            {
                Assert.IsTrue(result > max + 1);
            }
        }

        [TestMethod]
        public void InvalidParam()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                var dyn = new DynamicActivity<int>();
                dyn.Implementation = () => new TimeLoop
                {
                    Timer = new InArgument<TimeSpan>(TimeSpan.FromSeconds(0)),
                    Body = new ActivityAction
                    {
                        Handler = new Sequence { }
                    }
                };
                WorkflowInvoker.Invoke(dyn);
            });
        }
    }
}