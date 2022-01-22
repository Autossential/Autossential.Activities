using Autossential.Core.Enums;
using Autossential.Shared.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities.Test
{
    [TestClass]
    public class DataTableToTextTests
    {
        public static DataTable Initialize(bool empty)
        {
            if (empty)
                return DataTableHelper.CreateDataTable<object>(4);

            return DataTableHelper.CreateDataTable<object>(4, new[]
            {
                new object[]{"Hello", 100, true, new DateTime(2021, 12, 28, 15, 02, 33) },
                new object[]{"World", DBNull.Value, null, new DateTime(2021, 12, 28, 15, 02, 33) }
            });
        }

        [TestMethod]
        [DataRow(TextFormat.JSON, true)]
        [DataRow(TextFormat.JSON, false)]
        [DataRow(TextFormat.HTML, true)]
        [DataRow(TextFormat.HTML, false)]
        [DataRow(TextFormat.XML, true)]
        [DataRow(TextFormat.XML, false)]
        public void Default(TextFormat format, bool empty)
        {
            var result = WorkflowInvoker.Invoke(new DataTableToText { TextFormat = format }, GetArgs(Initialize(empty)));

            switch (format)
            {
                case TextFormat.HTML:
                    Assert.AreEqual(GetHTML(empty), result);
                    break;
                case TextFormat.JSON:
                    Assert.AreEqual(GetJSON(empty), result);
                    break;
                case TextFormat.XML:
                    Assert.AreEqual(GetXML(empty), result);
                    break;
            }
        }

        [TestMethod]
        [DataRow("dd/MM/yyyy")]
        [DataRow(null)]
        public void TypedColumnsAndDateTimeFormat(string dateTimeFormat)
        {
            var dt = new DataTable();
            dt.Columns.Add("A", typeof(DateTime));
            dt.Columns.Add("B", typeof(string));
            dt.Columns.Add("C", typeof(char));

            var row = dt.NewRow();
            row["A"] = new DateTime(2022, 2, 1);
            row["B"] = "Yes";
            row["C"] = "A";

            dt.Rows.Add(row);

            var result = WorkflowTester.Invoke(new DataTableToText
            {
                TextFormat = TextFormat.JSON,
                DateTimeFormat = dateTimeFormat
            }, GetArgs(dt));

            var dateRes = dateTimeFormat == null
                ? "2022-02-01T00:00:00.00000"
                : "01/02/2022";

            Assert.AreEqual(
                $@"[{{""A"":""{dateRes}"",""B"":""Yes"",""C"":""A""}}]",
                result
            );
        }

        [TestMethod]
        public void Binary()
        {
            var dt = new DataTable("MyTable");
            dt.Columns.Add("Bin", typeof(byte[]));
            var row = dt.NewRow();
            row["Bin"] = new byte[] { 1, 2, 3 }; // it converts to base64 representation
            dt.Rows.Add(row);

            var result = WorkflowTester.Invoke(new DataTableToText { TextFormat = TextFormat.XML }, GetArgs(dt));
            Assert.AreEqual(
                "<DocumentElement><MyTable><Bin>AQID</Bin></MyTable></DocumentElement>",
                result
            );
        }

        private static IDictionary<string, object> GetArgs(object dataTable)
        {
            return new Dictionary<string, object>
            {
                { nameof(DataTableToText.InputDataTable), dataTable }
            };
        }


        private static string GetJSON(bool empty)
        {
            if (empty) return "[]";

            return @"[{""Col0"":""Hello"",""Col1"":100,""Col2"":""true"",""Col3"":""2021-12-28T15:02:33.00000""},{""Col0"":""World"",""Col1"":null,""Col2"":null,""Col3"":""2021-12-28T15:02:33.00000""}]";
        }
        private static string GetHTML(bool empty)
        {
            if (empty) return "<table><thead><tr><th>Col0</th><th>Col1</th><th>Col2</th><th>Col3</th></tr></thead><tbody></tbody></table>";

            return "<table><thead><tr><th>Col0</th><th>Col1</th><th>Col2</th><th>Col3</th></tr></thead><tbody><tr><td>Hello</td><td>100</td><td>true</td><td>2021-12-28T15:02:33.00000</td></tr><tr><td>World</td><td></td><td></td><td>2021-12-28T15:02:33.00000</td></tr></tbody></table>";
        }
        private static string GetXML(bool empty)
        {
            if (empty) return "<DocumentElement></DocumentElement>";

            return "<DocumentElement><Table1><Col0>Hello</Col0><Col1>100</Col1><Col2>true</Col2><Col3>2021-12-28T15:02:33.00000</Col3></Table1><Table1><Col0>World</Col0><Col1 /><Col2 /><Col3>2021-12-28T15:02:33.00000</Col3></Table1></DocumentElement>";
        }

    }
}
