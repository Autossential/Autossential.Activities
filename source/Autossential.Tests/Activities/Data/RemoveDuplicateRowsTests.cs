using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class RemoveDuplicateRowsTests
    {
        private static DataTable Initialize()
        {
            return DataTableHelper.CreateDataTable<object>(3, new[]
            {
                new object[] { "A", 1, true },
                new object[] { "A", 2, true },

                new object[] { "B", 1, true },
                new object[] { "B", 2, true },

                new object[] { "C", 1, false },
                new object[] { "C", 1, true },
                new object[] { "C", 1, false },

                new object[] { "D", 2, false },
                new object[] { "D", 3, true },
            });
        }

        [TestMethod]
        [DataRow(null, 8)]
        [DataRow("0", 4)]
        [DataRow("1", 3)]
        [DataRow("2", 2)]
        [DataRow("1,2", 5)]
        [DataRow("0,2", 6)]

        public void Default(string cols, int expectedRowsCount)
        {
            var dt = Initialize();

            InArgument<int[]> colsArg = null;
            if (cols != null)
            {
                var values = cols.Split(',').Select(int.Parse).ToArray();
                colsArg = new InArgument<int[]>(_ => values);
            }


            var result = WorkflowTester.CompileAndInvoke(new RemoveDuplicateRows()
            {
                Columns = colsArg
            }, GetArgs(dt));

            Assert.AreEqual(expectedRowsCount, result.Rows.Count);
        }


        private static IDictionary<string, object> GetArgs(DataTable inputDataTable)
        {
            return new Dictionary<string, object>
            {
                { nameof(RemoveDuplicateRows.InputDataTable), inputDataTable }
            };
        }
    }
}
