using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class RemoveEmptyRowsTests
    {
        [Fact]
        public void Invoke_WithEmptyRows_RemovesEmptyRows()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            dt.Rows.Add("Alice", 100);
            dt.Rows.Add(null, null);
            dt.Rows.Add("Bob", 200);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            WorkflowInvoker.Invoke(new RemoveEmptyRows(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal("Alice", result.Rows[0][0]);
            Assert.Equal("Bob", result.Rows[1][0]);
        }

        [Fact]
        public void Invoke_WithColumnNames_OnlyChecksSpecifiedColumns()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            dt.Rows.Add("Alice", 100);
            dt.Rows.Add(null, 200);
            dt.Rows.Add("Bob", null);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["ColumnNames"] = new List<string> { "Name" }
            };

            WorkflowInvoker.Invoke(new RemoveEmptyRows(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal("Alice", result.Rows[0][0]);
            Assert.Equal("Bob", result.Rows[1][0]);
        }

        [Fact]
        public void Invoke_WithColumnIndexes_OnlyChecksSpecifiedIndexes()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            dt.Rows.Add("Alice", 100);
            dt.Rows.Add(null, null);
            dt.Rows.Add("Bob", 200);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["ColumnIndexes"] = new List<int> { 0 }
            };

            WorkflowInvoker.Invoke(new RemoveEmptyRows(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Rows.Count);
        }

        [Fact]
        public void Invoke_WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new RemoveEmptyRows(), inputs));
        }

        [Fact]
        public void Invoke_WithNoEmptyRows_DoesNotRemoveAnyRows()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            dt.Rows.Add("Alice", 100);
            dt.Rows.Add("Bob", 200);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            WorkflowInvoker.Invoke(new RemoveEmptyRows(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Rows.Count);
        }
    }
}
