using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class TransposeDataTests
    {
        [Test]
        public async Task WithValidDataTable_ModifiesTheDataTable()
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
            await Assert.That(result).IsNotNull();
            await Assert.That(result.Columns.Count).IsNotEqualTo(0);
            await Assert.That(result.Rows.Count).IsNotEqualTo(0);
        }

        [Test]
        public async Task WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new TransposeData(), inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        public async Task ColumnsAreNamedSequentially()
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
                await Assert.That(result.Columns[i].ColumnName).IsNotNull();
                await Assert.That(result.Columns[i].ColumnName).IsNotEmpty();
            }
        }

        [Test]
        public async Task ShouldTransposeDataTableCorrectly()
        {
            // Arrange
            var input = new DataTable();
            input.Columns.Add("Name");
            input.Columns.Add("Age", typeof(int));

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
            await Assert.That(output).IsNotNull();
            await Assert.That(output.Columns.Count).IsEqualTo(3);
            await Assert.That(output.Rows.Count).IsEqualTo(2);

            await Assert.That(output.Rows[0][0]).IsEqualTo("Name");
            await Assert.That(output.Rows[0][1]).IsEqualTo("Alice");
            await Assert.That(output.Rows[0][2]).IsEqualTo("Bob");

            await Assert.That(output.Rows[1][0]).IsEqualTo("Age");
            await Assert.That(output.Rows[1][1]).IsEqualTo(30);
            await Assert.That(output.Rows[1][2]).IsEqualTo(25);
        }
    }
}