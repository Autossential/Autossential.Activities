using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using TUnit;

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

        [Test]
        public async Task WithHTMLFormatDefault_ReturnsHTMLTable()
        {
            var dt = CreateSampleDataTable();

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.HTML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = WorkflowInvoker.Invoke(activity, inputs);
            await Assert.That(result).IsEqualTo(ExpectedHTML);
        }

        [Test]
        public async Task WithJSONFormatSet_ReturnsJSONText()
        {
            var dt = CreateSampleDataTable();

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.JSON };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = WorkflowInvoker.Invoke(activity, inputs);
            await Assert.That(result).IsEqualTo(ExpectedJSON);
        }

        [Test]
        public async Task WithXMLFormatSet_ReturnsXMLText()
        {
            var dt = CreateSampleDataTable();

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.XML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = WorkflowInvoker.Invoke(activity, inputs);
            await Assert.That(result).IsEqualTo(ExpectedXML);
        }

        [Test]
        public async Task WithEmptyDataTable_ReturnsEmptyString()
        {
            var dt = new DataTable();
            dt.Columns.Add("Name");

            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.HTML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = dt
            };

            var result = WorkflowInvoker.Invoke(activity, inputs);
            await Assert.That(result).IsEqualTo(string.Empty);
        }

        [Test]
        public async Task WithNullDataTable_ThrowsInvalidOperationException()
        {
            var activity = new DataTableToText { OutputFormat = DataTableToText.OutputTextFormat.HTML };
            var inputs = new Dictionary<string, object>
            {
                ["DataTable"] = null!
            };

            await Assert.That(() => WorkflowInvoker.Invoke(activity, inputs))
                .Throws<InvalidOperationException>();
        }

        [Test]
        [Arguments("dd-MMM-yyyy", typeof(DateTime))]
        [Arguments("dddd MMM yyyy", typeof(DateTime))]
        [Arguments("dd-MMM-yyyy", typeof(object))]
        [Arguments("dddd MMM yyyy", typeof(object))]
        public async Task WithDateFormating_FormatAccordingly(string dateFormat, Type colType)
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

            await Assert.That(result).Contains(date.ToString(dateFormat));
        }
    }
}