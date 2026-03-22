using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    public class PromoteHeadersTests
    {
        [Fact]
        public void Invoke_WithValidDataTable_PromotesFirstRowToHeaders()
        {
            var dt = new DataTable();
            dt.Columns.Add("Column0");
            dt.Columns.Add("Column1");
            dt.Rows.Add("Name", "Age");
            dt.Rows.Add("Alice", 30);
            dt.Rows.Add("Bob", 25);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["AutoRename"] = true
            };

            WorkflowInvoker.Invoke(new PromoteHeaders(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("Name", result.Columns[0].ColumnName);
            Assert.Equal("Age", result.Columns[1].ColumnName);
            Assert.Equal(2, result.Rows.Count);
            Assert.Equal("Alice", result.Rows[0][0]);
            // Both values are strings after promotion
            Assert.Equal("30", result.Rows[0][1]);
        }

        [Fact]
        public void Invoke_WithEmptyDataTable_ThrowsInvalidOperationException()
        {
            var dt = new DataTable();
            dt.Columns.Add("Column0");
            dt.Columns.Add("Column1");

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new PromoteHeaders(), inputs));
        }

        [Fact]
        public void Invoke_WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new PromoteHeaders(), inputs));
        }

        [Fact]
        public void Invoke_WithDuplicateHeaderNames_AutoRenameTrue_RenamesDuplicates()
        {
            var dt = new DataTable();
            dt.Columns.Add("Column0");
            dt.Columns.Add("Column1");
            dt.Columns.Add("Column2");
            dt.Rows.Add("Name", "Name", "Age");
            dt.Rows.Add("Alice", "Alice2", 30);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt,
                ["AutoRename"] = true
            };

            WorkflowInvoker.Invoke(new PromoteHeaders(), inputs);

            var result = (DataTable)inputs["DataTable"];
            Assert.NotEqual(result.Columns[0].ColumnName, result.Columns[1].ColumnName);
            Assert.Equal(3, result.Columns.Count);
        }
    }
}
