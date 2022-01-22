using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class DataRowToDictionaryTests
    {
        [TestMethod]
        public void Default()
        {
            var dt = DataTableHelper.CreateDataTable<string>(3, new[] {
                new object[]{"A", 10, true}
            });

            var row = dt.Rows[0];
            var result = WorkflowInvoker.Invoke(new DataRowToDictionary(), GetArgs(row));

            Assert.AreEqual(row[0], result["Col0"]);
            Assert.AreEqual(row[1], result["Col1"]);
            Assert.AreEqual(row[2], result["Col2"]);
        }

        private static Dictionary<string, object> GetArgs(System.Data.DataRow dataRow)
        {
            return new Dictionary<string, object>
            {
                { nameof(DataRowToDictionary.InputDataRow), dataRow },
            };
        }
    }
}
