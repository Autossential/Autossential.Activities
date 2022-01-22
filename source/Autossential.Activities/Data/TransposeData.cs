using Autossential.Shared;
using System.Activities;
using System.Data;

namespace Autossential.Activities
{
    public class TransposeData : CodeActivity<DataTable>
    {
        public InArgument<DataTable> InputDataTable { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(InputDataTable, nameof(InputDataTable), true);
            metadata.AddRuntimeArgument(Result, nameof(Result), true);
            base.CacheMetadata(metadata);
        }

        protected override DataTable Execute(CodeActivityContext context)
        {
            var input = InputDataTable.Get(context);
            var rowsCount = input.Rows.Count;

            var output = new DataTable();

            for (int i = 0; i <= rowsCount; i++)
                output.Columns.Add("Col" + (i + 1));

            foreach (DataColumn col in input.Columns)
            {
                var row = output.NewRow();
                row[0] = col.ColumnName;

                for (int i = 0; i < rowsCount; i++)
                    row[i + 1] = input.Rows[i][col.Ordinal];

                output.Rows.Add(row);
            }

            return output;
        }
    }
}
