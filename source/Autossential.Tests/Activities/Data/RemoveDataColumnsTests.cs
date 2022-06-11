using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class RemoveDataColumnsTests
    {
        [TestMethod]
        public void Default()
        {
            var dt = DataTableHelper.CreateDataTable<object>(7);

            WorkflowTester.CompileAndRun(new RemoveDataColumns
            {
                Columns = new InArgument<int[]>(_ => new int[] { 1, 3 })
            }, GetArgs(dt));

            // 0,1,2,3,4,5,6
            // 0,1,2,4,5,6   (index 3 removed)
            // 0,2,4,5,6     (index 1 removed)

            Assert.AreEqual(dt.Columns.Count, 5);
            Assert.AreEqual(dt.Columns[1].ColumnName, "Col2");
            Assert.AreEqual(dt.Columns[2].ColumnName, "Col4");
        }


        [TestMethod]
        public void InvalidColumnNames()
        {
            var dt = DataTableHelper.CreateDataTable<object>(3);

            WorkflowTester.CompileAndRun(new RemoveDataColumns
            {
                Columns = new InArgument<string[]>(_ => new string[] { "Col1", "Col4" })
            }, GetArgs(dt));

            Assert.AreEqual(dt.Columns.Count, 2);
            Assert.AreEqual(dt.Columns[0].ColumnName, "Col0");
            Assert.AreEqual(dt.Columns[1].ColumnName, "Col2");
        }

        [TestMethod]
        public void InvalidColumnIndexes()
        {
            var dt = DataTableHelper.CreateDataTable<object>(2);

            WorkflowTester.CompileAndRun(new RemoveDataColumns
            {
                Columns = new InArgument<int[]>(_ => new int[] { 2, 0, 3  })
            }, GetArgs(dt));

            Assert.AreEqual(dt.Columns.Count, 1);
            Assert.AreEqual(dt.Columns[0].ColumnName, "Col1");
        }

        private static IDictionary<string, object> GetArgs(DataTable dt)
        {
            return new Dictionary<string, object>
            {
                { nameof(RemoveDataColumns.ReferenceDataTable), dt }
            };
        }
    }
}
