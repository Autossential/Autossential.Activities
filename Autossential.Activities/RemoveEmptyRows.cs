using Autossential.Activities.Properties;
using System.Activities;
using System.Data;

namespace Autossential.Activities
{
    public sealed class RemoveEmptyRows : CodeActivity
    {
        public enum MatchMode
        {
            All,
            Any
        }

        [RequiredArgument]
        public InOutArgument<DataTable> DataTable { get; set; }
        public InArgument<IReadOnlyList<string>> ColumnNames { get; set; }
        public InArgument<IReadOnlyList<int>> ColumnIndexes { get; set; }
        public MatchMode MatchingMode { get; set; } = MatchMode.All;
        protected override void Execute(CodeActivityContext context)
        {
            var dt = DataTable.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.RemoveEmptyRows_DataTable_DisplayName));
            var names = ColumnNames.Get(context) ?? [];
            var indexes = ColumnIndexes.Get(context) ?? [];

            var rowsToRemove = new List<DataRow>();

            static bool isEmpty(object v) => v == DBNull.Value || string.IsNullOrEmpty(v?.ToString());

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<object> values = [];

                if (names.Count > 0)
                    values = values.Concat(names.Select(n => row[n]));

                if (indexes.Count > 0)
                    values = values.Concat(indexes.Select(i => row[i]));

                if (!values.Any())
                    values = row.ItemArray;

                if (MatchingMode == MatchMode.All ? values.All(isEmpty) : values.Any(isEmpty))
                {
                    rowsToRemove.Add(row);
                }
            }

            foreach (var row in rowsToRemove)
            {
                dt.Rows.Remove(row);
            }

            DataTable.Set(context, dt);
        }
    }
}