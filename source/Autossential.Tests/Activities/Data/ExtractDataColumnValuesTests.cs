using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class ExtractDataColumnValuesTests
    {
        private static DataTable Initialize()
        {
            return DataTableHelper.CreateDataTable(new[] { typeof(string), typeof(int) }, new[]
            {
                new object[] { "A", 1 },
                new object[] { "B", 2 },
                new object[] { "C", 3 }
            });
        }

        [TestMethod]
        public void DefaultString()
        {
            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<string>()
            {
                Column = new InArgument<string>("Col0")
            }, GetArgs(Initialize(), "Col0"));

            CollectionAssert.AreEqual(new[] { "A", "B", "C" }, result);
        }


        [TestMethod]
        public void DefaultInt()
        {
            var result = WorkflowTester.Invoke(new ExtractDataColumnValues<int>()
            {
                Column = new InArgument<int>(1)
            }, GetArgs(Initialize(), 1));

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [TestMethod]
        public void NullColumn()
        {
            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>(), GetArgs(Initialize(), 1)));
        }

        [TestMethod]
        public void InvalidColumnArgument()
        {
            Assert.ThrowsException<InvalidWorkflowException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>
            {
                Column = new InArgument<bool>(true)
            }, GetArgs(Initialize(), 1)));
        }

        [TestMethod]
        public void InvalidColumnReference()
        {
            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>
            {
                Column = new InArgument<string>("InvalidColName")
            }, GetArgs(Initialize(), 1)));

            Assert.ThrowsException<ArgumentException>(() => WorkflowTester.Invoke(new ExtractDataColumnValues<int>
            {
                Column = new InArgument<int>(3)
            }, GetArgs(Initialize(), 1)));
        }

        private static IDictionary<string, object> GetArgs(DataTable inputDataTable, object defaultValue)
        {
            return new Dictionary<string, object>
            {
                { nameof(ExtractDataColumnValues<object>.InputDataTable), inputDataTable },
                { nameof(ExtractDataColumnValues<object>.DefaultValue), defaultValue },
            };
        }
    }
}
