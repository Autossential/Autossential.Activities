using System.Activities;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Autossential.Activities
{
    public sealed class DataTableToText : CodeActivity<string>
    {
        public enum OutputTextFormat
        {
            HTML,
            JSON,
            XML
        }

        private const string DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss.fffff";

        [RequiredArgument]
        public InArgument<DataTable> DataTable { get; set; }
        public OutputTextFormat OutputFormat { get; set; } = OutputTextFormat.HTML;
        public InArgument<string> DateTimeFormat { get; set; } = DATE_FORMAT;

        protected override string Execute(CodeActivityContext context)
        {
            var dt = DataTable.Get(context) ?? throw new NullReferenceException("");
            if (dt.Rows.Count == 0)
                return string.Empty;

            var dateTimeFormat = DateTimeFormat.Get(context) ?? DATE_FORMAT;
            return OutputFormat switch
            {
                OutputTextFormat.HTML => ToHtml(dt, dateTimeFormat),
                OutputTextFormat.JSON => ToJson(dt, dateTimeFormat),
                OutputTextFormat.XML => ToXML(dt, dateTimeFormat),
                _ => null
            };
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
        private static string ToXML(DataTable dt, string dateTimeFormat)
        {
            var regex = new Regex("\\W+", RegexOptions.IgnoreCase);
            var sb = new StringBuilder();

            sb.Append("<DataTable>");
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendFormat("<Row>");
                foreach (DataColumn col in dt.Columns)
                {
                    if (row.IsNull(col.Ordinal) || string.IsNullOrEmpty(row[col.Ordinal].ToString()))
                    {
                        sb.AppendFormat("<{0} />", regex.Replace(col.ColumnName, "_"));
                        continue;
                    }

                    sb.AppendFormat("<{0}>{1}</{0}>", regex.Replace(col.ColumnName, "_"), FormatValue(row[col.Ordinal], dateTimeFormat).Replace("<", "&lt;").Replace(">", "&gt;"));
                }
                sb.AppendFormat("</Row>");
            }
            sb.Append("</DataTable>");

            return sb.ToString();
        }

        private static string ToJson(DataTable dt, string dateTimeFormat)
        {
            var sb = new StringBuilder();
            sb.Append('[');
            foreach (DataRow row in dt.Rows)
            {
                sb.Append('{');
                foreach (DataColumn col in dt.Columns)
                {
                    sb.AppendFormat("\"{0}\":", col.ColumnName.Replace("\"", "\\\""));
                    var value = FormatValue(row[col.ColumnName], dateTimeFormat);

                    if (col.DataType != typeof(bool)
                        && row[col.ColumnName] != DBNull.Value
                        && (col.DataType == typeof(DateTime)
                            || col.DataType == typeof(string)
                            || col.DataType == typeof(char)
                            || Regex.IsMatch(value, "[^.\\d]", RegexOptions.IgnoreCase)) || value.Length == 0)
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

            sb.Append(']');
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
                case byte[] bytesValue:
                    value = Convert.ToBase64String(bytesValue);
                    break;

                case DateTime dateValue:
                    value = dateValue.ToString(dateTimeFormat);
                    break;
            }

            return value.ToString();
        }
    }
}