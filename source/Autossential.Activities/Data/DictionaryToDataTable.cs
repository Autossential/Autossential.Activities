using Autossential.Shared;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities
{
    public sealed class DictionaryToDataTable : CodeActivity<DataTable>
    {
        public InArgument<Dictionary<string, object>> InputDictionary { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(Result, nameof(Result), true);
            metadata.AddRuntimeArgument(InputDictionary, "Dictionary", true);
            base.CacheMetadata(metadata);
        }

        protected override DataTable Execute(CodeActivityContext context)
        {
            var dictionary = InputDictionary.Get(context);
            var table = new DataTable();

            if (dictionary.Count > 0)
            {
                foreach (var item in dictionary)
                    table.Columns.Add(item.Key, item.Value?.GetType() ?? typeof(object));

                table.BeginLoadData();
                table.LoadDataRow(dictionary.Values.ToArray(), true);
                table.EndLoadData();
            }

            return table;
        }
    }
}