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
        [DataRow(3, true)]
        public void Default(int iterations, bool reverse)
        {
            var list = new List<int>();

            var index = new DelegateInArgument<int>("index");

            var dyn = new Sequence
            {
                Activities =
                {
                    new Iterate()
                    {
                        Iterations = iterations,
                        Reverse = reverse,
                        Body = new ActivityAction<int>
                        {
                            Argument = index,
                            Handler = new Sequence
                            {
                                Activities =
                                {
                                    new InvokeMethod()
                                    {
                                        TargetObject = new InArgument<List<int>>(_ => list),
                                        MethodName = "Add",
                                        Parameters = 
                                        { 
                                            new InArgument<int>(ctx => index.Get(ctx))
                                        }
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