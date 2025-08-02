using System.Activities;
using System.Data;

namespace Autossential.Activities
{
    public class TransposeData : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument<DataTable> DataTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var input = DataTable.Get(context);
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

            DataTable.Set(context, output);
        }
    }
}
