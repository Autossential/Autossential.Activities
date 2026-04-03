using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class DataTableToTextTests
    {
        private static DataTable CreateSampleDataTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Rows.Add("Alice", 30);
            dt.Rows.Add("Bob", 25);
            return dt;
        }

        [Fact]
        public void Invoke_WithHTMLFormatDefault_ReturnsHTMLTable()
        {
            var dt = CreateSampleDataTable();

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.HTML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = (string)WorkflowInvoker.Invoke(activity, inputs);

            Assert.NotNull(result);
            Assert.Contains("<table>", result);
            Assert.Contains("<tr>", result);
            Assert.Contains("<td>", result);
            Assert.Contains("Alice", result);
            Assert.Contains("Bob", result);
        }

        [Fact]
        public void Invoke_WithJSONFormatSet_ReturnsJSONText()
        {
            var dt = CreateSampleDataTable();

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.JSON };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = (string)WorkflowInvoker.Invoke(activity, inputs);

            Assert.NotNull(result);
            Assert.Contains("[", result);
            Assert.Contains("]", result);
            Assert.Contains("Alice", result);
        }

        [Fact]
        public void Invoke_WithXMLFormatSet_ReturnsXMLText()
        {
            var dt = CreateSampleDataTable();

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.XML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = (string)WorkflowInvoker.Invoke(activity, inputs);

            Assert.NotNull(result);
            Assert.Contains("<", result);
            Assert.Contains(">", result);
            Assert.Contains("Alice", result);
        }

        [Fact]
        public void Invoke_WithEmptyDataTable_ReturnsEmptyString()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.HTML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = (string)WorkflowInvoker.Invoke(activity, inputs);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Invoke_WithNullDataTable_ThrowsInvalidOperationException()
        {
            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.HTML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(activity, inputs));
        }
    }
}
