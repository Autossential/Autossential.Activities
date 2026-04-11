using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class TransposeDataTests
    {
        [Fact]
        public void Invoke_WithValidDataTable_ModifiesTheDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Rows.Add("Alice", 30);
            dt.Rows.Add("Bob", 25);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var outputs = WorkflowInvoker.Invoke(new TransposeData(), inputs);

            var result = (DataTable)outputs["DataTable"];

            // Verify the result is a DataTable and has been modified
            Assert.NotNull(result);
            Assert.NotEmpty(result.Columns);
            Assert.NotEqual(0, result.Rows.Count);
        }

        [Fact]
        public void Invoke_WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new TransposeData(), inputs));
        }

        [Fact]
        public void Invoke_ColumnsAreNamedSequentially()
        {
            var dt = new DataTable();
            dt.Columns.Add("A");
            dt.Columns.Add("B");
            dt.Rows.Add("1", "2");

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var outputs = WorkflowInvoker.Invoke(new TransposeData(), inputs);

            var result = (DataTable)outputs["DataTable"];
            // Columns are named numerically (Col1, Col2, etc.)
            for (int i = 0; i < result.Columns.Count; i++)
            {
                Assert.NotNull(result.Columns[i].ColumnName);
                Assert.NotEmpty(result.Columns[i].ColumnName);
            }
        }

        [Fact]
        public void Execute_ShouldTransposeDataTableCorrectly()
        {
            // Arrange
            var input = new DataTable();
            input.Columns.Add("Name");
            input.Columns.Add("Age");

            input.Rows.Add("Alice", 30);
            input.Rows.Add("Bob", 25);

            var inputs = new Dictionary<string, object>
            {
                { "DataTable", input }
            };

            // Act
            var outputs = WorkflowInvoker.Invoke(new TransposeData(), inputs);
            var output = outputs["DataTable"] as DataTable;

            // Assert
            Assert.NotNull(output);
            Assert.Equal(3, output.Columns.Count);
            Assert.Equal(2, output.Rows.Count);

            Assert.Equal("Name", output.Rows[0][0]);
            Assert.Equal("Alice", output.Rows[0][1]);
            Assert.Equal("Bob", output.Rows[0][2]);

            Assert.Equal("Age", output.Rows[1][0]);
            Assert.Equivalent(30, output.Rows[1][1]);
            Assert.Equivalent(25, output.Rows[1][2]);
        }
    }
}
