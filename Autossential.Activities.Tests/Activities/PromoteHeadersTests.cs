using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class PromoteHeadersTests
    {
        [Test]
        public async Task WithValidDataTable_PromotesFirstRowToHeaders()
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
            await Assert.That(result.Columns.Count).IsEqualTo(2);
            await Assert.That(result.Columns[0].ColumnName).IsEqualTo("Name");
            await Assert.That(result.Columns[1].ColumnName).IsEqualTo("Age");
            await Assert.That(result.Rows.Count).IsEqualTo(2);
            await Assert.That(result.Rows[0][0]).IsEqualTo("Alice");
            // Both values are strings after promotion
            await Assert.That(result.Rows[0][1]).IsEqualTo("30");
        }

        [Test]
        public async Task WithEmptyDataTable_ThrowsInvalidOperationException()
        {
            var dt = new DataTable();
            dt.Columns.Add("Column0");
            dt.Columns.Add("Column1");

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new PromoteHeaders(), inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        public async Task WithNullDataTable_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new PromoteHeaders(), inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        public async Task WithDuplicateHeaderNames_AutoRenameTrue_RenamesDuplicates()
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
            await Assert.That(result.Columns[0].ColumnName).IsNotEqualTo(result.Columns[1].ColumnName);
            await Assert.That(result.Columns.Count).IsEqualTo(3);
        }
    }
}