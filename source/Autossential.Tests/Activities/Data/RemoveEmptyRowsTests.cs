using Autossential.Activities;
using Autossential.Core.Enums;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Tests
{
    [TestClass]
    public class RemoveEmptyRowsTests
    {
        private static DataTable Initialize()
        {
            return DataTableHelper.CreateDataTable<object>(3, new[]
            {
                new object[] { "A", "B", "C" },
                new object[] { null, null, null },
                new object[] {  "", "", "" },
                new object[] { "", "B", "C" },
                new object[] { "A", "", "C" },
                new object[] { null, "B", null },
                new object[] { "A", "", null }
            });
        }

        [TestMethod]
        [DataRow(DataRowEvaluationMode.All, 5)]
        [DataRow(DataRowEvaluationMode.Any, 1)]

        public void Default(DataRowEvaluationMode valuesMode, int expectedCount)
        {
            //var dt = Initialize();

            //var result = WorkflowTester.Invoke(new RemoveEmptyRows()
            //{
            //    Mode = valuesMode
            //}, GetArgs(dt));

            //Assert.AreEqual(expectedCount, result.Rows.Count);
        }

        [TestMethod]
        [DataRow("0,1", ConditionOperator.And, 5)]
        [DataRow("0,1", ConditionOperator.Or, 1)]
        [DataRow("0", ConditionOperator.And, 3)] // for one column only the operator is irrelevant
        [DataRow("1,2", ConditionOperator.And, 4)]
        [DataRow("0,2", ConditionOperator.Or, 2)]
        public void Custom(string cols, ConditionOperator op, int expectedCount)
        {
            //var dt = Initialize();
            //var values = cols.Split(',').Select(int.Parse).ToArray();
            //var result = WorkflowTester.CompileAndInvoke(new RemoveEmptyRows
            //{
            //    Columns = new InArgument<int[]>(_ => values),
            //    Mode = DataRowEvaluationMode.Custom,
            //    Operator = op,
            //}, GetArgs(dt));

            //Assert.AreEqual(expectedCount, result.Rows.Count);
        }


        //private static IDictionary<string, object> GetArgs(DataTable inputDataTable)
        //{
        //    return new Dictionary<string, object>
        //    {
        //        { nameof(RemoveEmptyRows.InputDataTable), inputDataTable }
        //    };
        //}
    }
}
