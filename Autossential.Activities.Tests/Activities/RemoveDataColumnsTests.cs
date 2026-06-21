using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class RemoveDataColumnsTests
    {
        [Test]
        public async Task WithColumnNames_RemovesSpecifiedColumns()
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
            await Assert.That(result.Columns.Count).IsEqualTo(2);
            await Assert.That(result.Columns[0].ColumnName).IsEqualTo("Name");
            await Assert.That(result.Columns[1].ColumnName).IsEqualTo("Email");
        }

        [Test]
        public async Task WithColumnIndexes_RemovesSpecifiedIndexes()
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
            await Assert.That(result.Columns.Count).IsEqualTo(2);
            await Assert.That(result.Columns[0].ColumnName).IsEqualTo("Name");
            await Assert.That(result.Columns[1].ColumnName).IsEqualTo("Email");
        }

        [Test]
        public async Task WithBothColumnNamesAndIndexes_RemovesBoth()
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
            await Assert.That(result.Columns.Count).IsEqualTo(2);
            await Assert.That(result.Columns[0].ColumnName).IsEqualTo("Name");
            await Assert.That(result.Columns[1].ColumnName).IsEqualTo("Email");
        }

        [Test]
        public async Task WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!,
                ["ColumnNames"] = new List<string> { "Age" }
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new RemoveDataColumns(), inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        public async Task WithInvalidColumnIndex_IgnoresInvalidIndex()
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
            await Assert.That(result.Columns.Count).IsEqualTo(2);
        }
    }
}