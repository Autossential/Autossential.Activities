using Autossential.Shared;
using System.Activities;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities
{
    public sealed class DataRowToDictionary : CodeActivity<Dictionary<string, object>>
    {
        public InArgument<DataRow> InputDataRow { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(InputDataRow, nameof(InputDataRow), true);
            metadata.AddRuntimeArgument(Result, nameof(Result), false);
        }

        protected override Dictionary<string, object> Execute(CodeActivityContext context)
        {
            var dataRow = InputDataRow.Get(context);
            var dictionary = new Dictionary<string, object>();

            foreach (DataColumn col in dataRow.Table.Columns)
                dictionary.Add(col.ColumnName, dataRow[col.ColumnName]);

            return dictionary;
        }
    }
}