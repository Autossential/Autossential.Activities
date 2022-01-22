using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Activities.Statements;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class ContainerTests
    {
        [TestMethod]
        public void Default()
        {
            var tester = new ContainerTester();
            var container = new Container();

            container.Activities.Add(tester.PlusOne());
            container.Activities.Add(tester.PlusOne());
            container.Activities.Add(new Exit());
            container.Activities.Add(tester.PlusOne());

            tester.SetContainer(container);

            var result = WorkflowTester.Invoke(tester);
            Assert.AreEqual(2, result);
        }


        [TestMethod]
        [DataRow(false, 2)]
        [DataRow(true, 1)]
        public void Condition(bool condition, int expected)
        {
            var tester = new ContainerTester();
            var container = new Container();

            container.Activities.Add(tester.PlusOne());
            container.Activities.Add(new Exit { Condition = new InArgument<bool>(condition) });
            container.Activities.Add(tester.PlusOne());

            tester.SetContainer(container);

            var result = WorkflowTester.Invoke(tester);
            Assert.AreEqual(expected, result);
        }


        private class ContainerTester : Activity<int>
        {
            public Assign<int> PlusOne()
            {
                return new Assign<int>
                {
                    To = new OutArgument<int>(env => Result.Get(env)),
                    Value = new InArgument<int>(env => Result.Get(env) + 1)
                };
            }

            public void SetContainer(Container container)
            {
                Implementation = () => container;
            }
        }
    }
}
