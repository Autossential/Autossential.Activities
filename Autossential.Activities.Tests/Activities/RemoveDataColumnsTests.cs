using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class RemoveDataColumnsTests
    {
        [Fact]
        public void Invoke_WithColumnNames_RemovesSpecifiedColumns()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Email");
            dt.Rows.Add("Alice", 30, "alice@test.com");
            dt.Rows.Add("Bob", 25, "bob@test.com");

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["ColumnNames"] = new List<string> { "Age" }
            };

            WorkflowInvoker.Invoke(new RemoveDataColumns(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Name", result.Columns[0].ColumnName);
            Assert.Equal("Email", result.Columns[1].ColumnName);
        }

        [Fact]
        public void Invoke_WithColumnIndexes_RemovesSpecifiedIndexes()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Email");
            dt.Rows.Add("Alice", 30, "alice@test.com");

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["ColumnIndexes"] = new List<int> { 1 }
            };

            WorkflowInvoker.Invoke(new RemoveDataColumns(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Name", result.Columns[0].ColumnName);
            Assert.Equal("Email", result.Columns[1].ColumnName);
        }

        [Fact]
        public void Invoke_WithBothColumnNamesAndIndexes_RemovesBoth()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Email");
            dt.Columns.Add("Phone");
            dt.Rows.Add("Alice", 30, "alice@test.com", "123-456");

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["ColumnNames"] = new List<string> { "Age" },
                ["ColumnIndexes"] = new List<int> { 3 }
            };

            WorkflowInvoker.Invoke(new RemoveDataColumns(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Name", result.Columns[0].ColumnName);
            Assert.Equal("Email", result.Columns[1].ColumnName);
        }

        [Fact]
        public void Invoke_WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!,
                ["ColumnNames"] = new List<string> { "Age" }
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new RemoveDataColumns(), inputs));
        }

        [Fact]
        public void Invoke_WithInvalidColumnIndex_IgnoresInvalidIndex()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Rows.Add("Alice", 30);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["ColumnIndexes"] = new List<int> { 10 }
            };

            WorkflowInvoker.Invoke(new RemoveDataColumns(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Columns.Count);
        }
    }
}
