using Autossential.Core.Enums;
using Autossential.Shared;
using System;
using System.Activities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Autossential.Activities
{
    public sealed class DataTableToText : CodeActivity<string>
    {
        private const string DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffff";
        public InArgument<DataTable> InputDataTable { get; set; }
        public TextFormat TextFormat { get; set; } = TextFormat.HTML;
        public InArgument<string> DateTimeFormat { get; set; } = DATE_FORMAT;
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(InputDataTable, nameof(InputDataTable), true);
            metadata.AddRuntimeArgument(DateTimeFormat, nameof(DateTimeFormat), false);
            metadata.AddRuntimeArgument(Result, nameof(Result), true);
        }

        protected override string Execute(CodeActivityContext context)
        {
            var dt = InputDataTable.Get(context);
            if (dt == null)
                return null;

            return ConvertTo(TextFormat, dt, DateTimeFormat.Get(context) ?? DATE_FORMAT);
        }

        private string ConvertTo(TextFormat textFormat, DataTable dt, string dateFormat)
        {
            switch (textFormat)
            {
                case TextFormat.HTML: return ToHtml(dt, dateFormat);
                case TextFormat.JSON: return ToJson(dt, dateFormat);
                case TextFormat.XML: return ToXML(dt, dateFormat);
                default: return null;
            }
        }

        private string ToJson(DataTable dt, string dateTimeFormat)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("{");
                foreach (DataColumn col in dt.Columns)
                {
                    sb.AppendFormat("\"{0}\":", col.ColumnName.Replace("\"", "\\\""));
                    var value = FormatValue(row[col.ColumnName], dateTimeFormat);

                    if (col.DataType != typeof(bool)
                        && row[col.ColumnName] != DBNull.Value
                        && (col.DataType == typeof(DateTime)
                            || col.DataType == typeof(string)
                            || col.DataType == typeof(char)
                            || Regex.IsMatch(value, "[^.\\d]", RegexOptions.IgnoreCase)))
                    {
                        // quotes required
                        value = $"\"{value}\"";
                    }

                    sb.AppendFormat("{0},", value);
                }
                sb.Length -= 1;
                sb.Append("},");
            }

            if (sb.Length > 1)
                sb.Length -= 1;

            sb.Append("]");
            return sb.ToString();
        }

        private static string ToXML(DataTable dt, string dateTimeFormat)
        {
            var name = string.IsNullOrEmpty(dt.TableName) ? "Table1" : dt.TableName;

            var sb = new StringBuilder();

            sb.Append("<DocumentElement>");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<{0}>", name);
                foreach (DataColumn col in dt.Columns)
                {
                    if (row.IsNull(col.Ordinal) || string.IsNullOrEmpty(row[col.Ordinal].ToString()))
                    {
                        sb.AppendFormat("<{0} />", col.ColumnName);
                        continue;
                    }

                    sb.AppendFormat("<{0}>{1}</{0}>", col.ColumnName, FormatValue(row[col.Ordinal], dateTimeFormat).Replace("<", "&lt;").Replace(">", "&gt;"));
                }
                sb.AppendFormat("</{0}>", name);
            }
            sb.Append("</DocumentElement>");

            return sb.ToString();
        }
        private static string ToHtml(DataTable dt, string dateTimeFormat)
        {
            var sb = new StringBuilder()
                .Append("<table>")
                .Append("<thead>")
                .Append("<tr>");

            foreach (DataColumn col in dt.Columns)
                sb.AppendFormat("<th>{0}</th>", col.ColumnName);

            sb.Append("</tr>")
                .Append("</thead>")
                .Append("<tbody>");

            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn col in dt.Columns)
                {
                    if (row.IsNull(col.Ordinal))
                    {
                        sb.Append("<td></td>");
                        continue;
                    }

                    var value = row[col.Ordinal];
                    sb.AppendFormat("<td>{0}</td>", FormatValue(value, dateTimeFormat).Replace("<", "&lt;").Replace(">", "&gt;"));
                }
                sb.Append("</tr>");
            }

            sb.Append("</tbody>")
             .Append("</table>");

            return sb.ToString();
        }
        private static string FormatValue(object value, string dateTimeFormat)
        {
            if (value == DBNull.Value)
                return "null";

            if (value is bool)
                return value.ToString().ToLower();

            if (value.GetType().IsPrimitive || value is decimal)
                return value.ToString();

            switch (value)
            {
                case DateTime dateValue:
                    value = dateValue.ToString(dateTimeFormat);
                    break;
                case byte[] bytesValue:
                    value = Convert.ToBase64String(bytesValue);
                    break;
            }

            return value.ToString();
        }
    }
}
