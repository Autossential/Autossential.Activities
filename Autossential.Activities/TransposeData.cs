using Autossential.Activities.Properties;
using System.Activities;
using System.Data;

namespace Autossential.Activities
{
    public sealed class TransposeData : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument<DataTable> DataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var input = DataTable.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.TransposeData_DataTable_DisplayName));
            var rowsCount = input.Rows.Count;

            var output = new DataTable();

            for (int i = 0; i <= rowsCount; i++)
                output.Columns.Add("Col" + (i + 1), typeof(object));

            foreach (DataColumn col in input.Columns)
            {
                var row = output.NewRow();
                row[0] = col.ColumnName;

                for (int i = 0; i < rowsCount; i++)
                    row[i + 1] = input.Rows[i][col.Ordinal];

                output.Rows.Add(row);
            }

            DataTable.Set(context, output);
        }
    }
}