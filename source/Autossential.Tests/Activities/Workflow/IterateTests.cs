using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class IterateTests
    {
        [TestMethod]
        [DataRow(3, false)]
        public void Default(int iterations, bool reverse)
        {
            var list = new List<int>();
            var index = 0;
            var dyn = new Sequence
            {
                Activities =
                {
                    new Iterate()
                    {
                        Iterations = iterations,
                        Reverse = reverse,
                        Index = new OutArgument<int>(_ => index),
                        Body = new ActivityAction
                        {
                            Handler = new Sequence
                            {
                                Variables = { new Variable<int>("index") },
                                Activities =
                                {
                                    new InvokeMethod
                                    {
                                        TargetObject = new InArgument<List<int>>(_ => list),
                                        MethodName = "Add",
                                        Parameters = { new InArgument<int>(_ => index) }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            WorkflowTester.CompileAndRun(dyn);

            var values = Enumerable.Range(0, iterations);
            if (reverse)
                values = values.Reverse();

            CollectionAssert.AreEqual(values.ToArray(), list);

        }
    }
}