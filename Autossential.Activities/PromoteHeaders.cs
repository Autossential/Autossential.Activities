using Autossential.Activities.Properties;
using System.Activities;
using System.Data;

namespace Autossential.Activities
{
    public sealed class PromoteHeaders : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument<DataTable> DataTable { get; set; }

        public InArgument<bool> AutoRename { get; set; } = true;

        protected override void Execute(CodeActivityContext context)
        {
            var autoRename = AutoRename.Get(context);

            var dt = DataTable.Get(context);
            if (dt.Rows.Count == 0)
                throw new InvalidOperationException(Resources.PromoteHeaders_ErrorMsg_NoData);

            var colNames = dt.Rows[0].ItemArray.Select(v => v.ToString()).ToArray();

            var emptyColIndex = 0;
            while (colNames.Any(name => $"_{emptyColIndex}".Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                emptyColIndex++;
            }

            var cache = new Dictionary<string, int>();
            if (autoRename)
            {
                for (int i = 0; i < colNames.Length; i++)
                {
                    var name = string.IsNullOrEmpty(colNames[i]) ? $"_{emptyColIndex}" : colNames[i];
                    if (cache.ContainsKey(name))
                    {
                        cache[name]++;
                        name += cache[name].ToString();
                    }
                    else
                    {
                        cache.Add(name, 0);
                    }
                    dt.Columns[i].ColumnName = name;
                }

            }
            else
            {
                for (int i = 0; i < colNames.Length; i++)
                {
                    var name = string.IsNullOrEmpty(colNames[i]) ? $"_{emptyColIndex}" : colNames[i];
                    dt.Columns[i].ColumnName = name;
                }
            }

            dt.Rows.RemoveAt(0);
            DataTable.Set(context, dt);
        }
    }
}