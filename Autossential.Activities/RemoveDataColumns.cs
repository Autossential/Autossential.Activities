using Autossential.Activities.Properties;
using System.Activities;
using System.Data;

namespace Autossential.Activities
{
    public sealed class RemoveDataColumns : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument<DataTable> DataTable { get; set; }
        public InArgument<IReadOnlyList<string>> ColumnNames { get; set; }
        public InArgument<IReadOnlyList<int>> ColumnIndexes { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var dt = DataTable.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.RemoveDataColumns_DataTable_DisplayName));
            var names = ColumnNames.Get(context) ?? [];
            var indexes = ColumnIndexes.Get(context) ?? [];

            indexes = [.. indexes, .. names.Select(n => dt.Columns.IndexOf(n))];
            var orderedIndexes = indexes.Where(i => i >= 0 && i < dt.Columns.Count).OrderByDescending(i => i).ToHashSet();
            foreach (var colIndex in orderedIndexes)
                dt.Columns.RemoveAt(colIndex);

            DataTable.Set(context, dt);
        }
    }
}