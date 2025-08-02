using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Tests
{
    [TestClass]
    public class TransposeDataTests
    {
        private static DataTable Initialize()
        {
            var dt = new DataTable();
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Color", typeof(string));

            dt.Rows.Add(new object[] { "A", 1, "Red" });
            dt.Rows.Add(new object[] { "B", 2, "Green" });
            dt.Rows.Add(new object[] { "C", 3, "Blue" });

            return dt;
        }

        //[TestMethod]
        //public void Default()
        //{
        //    var result = WorkflowTester.Invoke(new TransposeData(), GetArgs(Initialize()));
        //    Assert.AreEqual("Col1", result.Columns[0].ColumnName);
        //    Assert.AreEqual("Col2", result.Columns[1].ColumnName);
        //    Assert.AreEqual("Col3", result.Columns[2].ColumnName);
        //    Assert.AreEqual("Col4", result.Columns[3].ColumnName);
        //    CollectionAssert.AreEqual(new[] { "Product", "A", "B", "C" }, result.Rows[0].ItemArray);
        //    CollectionAssert.AreEqual(new object[] { "Quantity", "1", "2", "3" }, result.Rows[1].ItemArray);
        //    CollectionAssert.AreEqual(new[] { "Color", "Red", "Green", "Blue" }, result.Rows[2].ItemArray);
        //}

        //private static IDictionary<string, object> GetArgs(DataTable dataTable)
        //{
        //    return new Dictionary<string, object> {
        //        { nameof(TransposeData.InputDataTable), dataTable }
        //    };
        //}
    }
}
