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

        private const string ExpectedHTML = "<table><thead><tr><th>Name</th><th>Age</th></tr></thead><tbody><tr><td>Alice</td><td>30</td></tr><tr><td>Bob</td><td>25</td></tr></tbody></table>";
        private const string ExpectedJSON = "[{\"Name\":\"Alice\",\"Age\":\"30\"},{\"Name\":\"Bob\",\"Age\":\"25\"}]";
        private const string ExpectedXML = "<DataTable><Row><Name>Alice</Name><Age>30</Age></Row><Row><Name>Bob</Name><Age>25</Age></Row></DataTable>";

        [Fact]
        public void Invoke_WithHTMLFormatDefault_ReturnsHTMLTable()
        {
            var dt = CreateSampleDataTable();

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.HTML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = WorkflowInvoker.Invoke(activity, inputs);
            Assert.Equal(ExpectedHTML, result);
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

            var result = WorkflowInvoker.Invoke(activity, inputs);
            Assert.Equal(ExpectedJSON, result);
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

            var result = WorkflowInvoker.Invoke(activity, inputs);
            Assert.Equal(ExpectedXML, result);
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

            var result = WorkflowInvoker.Invoke(activity, inputs);
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

        [Theory]
        [InlineData("dd-MMM-yyyy", typeof(DateTime))]
        [InlineData("dddd MMM yyyy", typeof(DateTime))]
        [InlineData("dd-MMM-yyyy", typeof(object))]
        [InlineData("dddd MMM yyyy", typeof(object))]
        public void Invoke_WithDateFormating_FormatAccordingly(string dateFormat, Type colType)
        {
            var date = DateTime.Now;
            var dt = new DataTable();
            dt.Columns.Add("Date", colType);
            var row = dt.NewRow();
            row["Date"] = date;
            dt.Rows.Add(row);

            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = WorkflowInvoker.Invoke(new DataTableToText
            {
                DateTimeFormat = dateFormat
            }, inputs);

            Assert.Contains(date.ToString(dateFormat), result);
        }
    }
}
