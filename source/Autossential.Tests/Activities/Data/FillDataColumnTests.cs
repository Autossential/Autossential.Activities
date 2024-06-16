using Autossential.Activities;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Tests
{
    [TestClass]
    public class FillDataColumnTests
    {
        private const string ColNameToSearch = "Col0";
        private const int ColIndexToSearch = 1;

        private DataTable GetTable()
        {
            return DataTableHelper.Generate(100, 3, (i, j) => $"row_{i}_col_{j}");
        }

        [TestMethod]
        public void FillWithNullValue()
        {
            var dt = GetTable();
            WorkflowTester.Invoke(new FillDataColumn
            {
                Column = new InArgument<string>(ColNameToSearch),
                Value = new InArgument<object>(_ => null)
            }, GetArgs(dt));

            Assert.IsTrue(dt.AsEnumerable().Select(row => row[ColNameToSearch]).All(value => value == DBNull.Value));
        }

        [TestMethod]
        public void FillWithoutValue()
        {
            var dt = GetTable();
            WorkflowTester.Invoke(new FillDataColumn
            {
                Column = new InArgument<string>(ColNameToSearch)
            }, GetArgs(dt));

            Assert.IsTrue(dt.AsEnumerable().Select(row => row[ColNameToSearch]).All(value => value == DBNull.Value));
        }

        [TestMethod]
        public void FillUsingColumnName()
        {
            var dt = GetTable();
            WorkflowTester.Invoke(new FillDataColumn
            {
                Column = new InArgument<string>(ColNameToSearch),
                Value = new InArgument<string>("-")
            }, GetArgs(dt));

            Assert.AreEqual(dt.Rows.Count, dt.Select($"[{ColNameToSearch}]='-'").Length);
        }

        [TestMethod]
        public void FillUsingColumnIndex()
        {
            var dt = GetTable();
            WorkflowTester.Invoke(new FillDataColumn
            {
                Column = new InArgument<int>(ColIndexToSearch),
                Value = new InArgument<string>("-")
            }, GetArgs(dt));

            Assert.AreEqual(dt.Rows.Count, dt.AsEnumerable().Count(row => row[ColIndexToSearch].ToString() == "-"));
        }

        [TestMethod]
        public void TryToFillWithInvalidColumnName()
        {
            var dt = GetTable();
            Assert.ThrowsException<ArgumentException>(() =>
            {
                WorkflowTester.Invoke(new FillDataColumn
                {
                    Column = new InArgument<string>("ColumnNameThatDoesNotExist"),
                    Value = new InArgument<string>("-")
                }, GetArgs(dt));
            });
        }

        [TestMethod]
        public void TryToFillWithInvalidColumnIndex()
        {
            var dt = GetTable();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                WorkflowTester.Invoke(new FillDataColumn
                {
                    Column = new InArgument<int>(int.MaxValue),
                    Value = new InArgument<string>("-")
                }, GetArgs(dt));
            });
        }

        private static Dictionary<string, object> GetArgs(object inputDataTable)
        {
            return new Dictionary<string, object>
            {
                { nameof(FillDataColumn.ReferenceDataTable), inputDataTable }
            };
        }
    }
}
