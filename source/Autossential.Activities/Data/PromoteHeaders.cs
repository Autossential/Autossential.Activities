using Autossential.Activities.Properties;
using Autossential.Shared;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;

namespace Autossential.Activities
{
    public sealed class PromoteHeaders : CodeActivity<DataTable>
    {
        public InArgument<DataTable> InputDataTable { get; set; }

        public InArgument<bool> AutoRename { get; set; } = true;

        public InArgument<string> EmptyColumnName { get; set; } = new InArgument<string>("Empty");

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(InputDataTable, nameof(InputDataTable), true);
            metadata.AddRuntimeArgument(EmptyColumnName, nameof(EmptyColumnName), true);
            metadata.AddRuntimeArgument(Result, nameof(Result), true);

            if (EmptyColumnName != null && EmptyColumnName.Expression is Literal<string> prop && string.IsNullOrEmpty(prop.Value))
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(EmptyColumnName)));
        }

        protected override DataTable Execute(CodeActivityContext context)
        {
            var names = new Dictionary<string, int>();
            var emptyName = EmptyColumnName.Get(context);
            var autoRename = AutoRename.Get(context);

            var inputDT = InputDataTable.Get(context);
            if (inputDT.Rows.Count == 0)
                throw new InvalidOperationException(Resources.PromoteHeaders_ErrorMsg_NoData);

            var outputDT = inputDT.Copy();

            var row = outputDT.Rows[0];

            string getName(string firstRowValue)
            {
                return string.IsNullOrEmpty(firstRowValue) ? emptyName : firstRowValue;
            }

            if (autoRename)
            {
                foreach (DataColumn col in outputDT.Columns)
                {
                    var name = getName(row[col.ColumnName].ToString());
                    if (names.ContainsKey(name))
                    {
                        names[name]++;
                        name += names[name].ToString();
                    }
                    else
                    {
                        names.Add(name, 0);
                    }

                    col.ColumnName = name;
                }
            }
            else
            {
                foreach (DataColumn col in outputDT.Columns)
                    col.ColumnName = getName(row[col.ColumnName].ToString());
            }

            outputDT.Rows.Remove(row);
            return outputDT;
        }
    }
}