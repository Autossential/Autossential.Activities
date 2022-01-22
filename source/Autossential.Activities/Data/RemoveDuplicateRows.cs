using Autossential.Activities.Properties;
using Autossential.Shared;
using Autossential.Shared.Utils;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities
{
    public sealed class RemoveDuplicateRows : CodeActivity<DataTable>
    {
        public InArgument<DataTable> InputDataTable { get; set; }

        public InArgument Columns { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(InputDataTable, nameof(InputDataTable), true);
            metadata.AddRuntimeArgument(Result, nameof(Result), true);

            if (Columns == null) return;
            if (Columns.IsArgumentTypeAnyCompatible<IEnumerable<int>, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(Columns, Columns.ArgumentType, nameof(Columns), true);
            }
            else
            {
                metadata.AddValidationError(Resources.Validation_TypeErrorFormat("IEnumerable<string> or IEnumerable<int>", nameof(Columns)));
            }
        }

        protected override DataTable Execute(CodeActivityContext context)
        {
            var inputDt = InputDataTable.Get(context) ?? throw new ArgumentException(nameof(InputDataTable));
            var columns = DataTableUtil.IdentifyDataColumns(inputDt, Columns?.Get(context));

            DataTable outputDt;
            if (columns.Any())
            {
                outputDt = inputDt.Clone();
                var colIndexes = columns.ToArray();
                foreach (DataRow inRow in inputDt.Rows)
                {
                    var skip = false;
                    foreach (DataRow outRow in outputDt.Rows)
                    {
                        if (RowExist(inRow.ItemArray, outRow.ItemArray, colIndexes))
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (skip) continue;

                    outputDt.ImportRow(inRow);
                }
            }
            else
            {
                outputDt = inputDt.AsDataView().ToTable(true);
            }

            return outputDt;
        }

        private bool RowExist(object[] inputValues, object[] outputValues, int[] columns)
        {
            bool flag = true;
            foreach (var colIndex in columns)
                flag &= Equals(inputValues[colIndex], outputValues[colIndex]);

            return flag;
        }
    }
}