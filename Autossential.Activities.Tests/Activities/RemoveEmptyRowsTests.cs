using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class RemoveEmptyRowsTests
    {
        [Test]
        public async Task WithEmptyRows_RemovesEmptyRows()
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
            await Assert.That(result.Rows.Count).IsEqualTo(2);
            await Assert.That(result.Rows[0][0]).IsEqualTo("Alice");
            await Assert.That(result.Rows[1][0]).IsEqualTo("Bob");
        }

        [Test]
        public async Task WithColumnNames_OnlyChecksSpecifiedColumns()
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
            await Assert.That(result.Rows.Count).IsEqualTo(2);
            await Assert.That(result.Rows[0][0]).IsEqualTo("Alice");
            await Assert.That(result.Rows[1][0]).IsEqualTo("Bob");
        }

        [Test]
        public async Task WithColumnIndexes_OnlyChecksSpecifiedIndexes()
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
            await Assert.That(result.Rows.Count).IsEqualTo(2);
        }

        [Test]
        public async Task WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new RemoveEmptyRows(), inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        public async Task WithNoEmptyRows_DoesNotRemoveAnyRows()
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
            await Assert.That(result.Rows.Count).IsEqualTo(2);
        }
    }
}