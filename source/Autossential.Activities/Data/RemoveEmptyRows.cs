using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using Autossential.Shared;
using Autossential.Shared.Utils;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities
{
    public sealed class RemoveEmptyRows : CodeActivity<DataTable>
    {
        public InArgument<DataTable> InputDataTable { get; set; }

        public DataRowEvaluationMode Mode { get; set; }
        public InArgument Columns { get; set; }
        public ConditionOperator Operator { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(InputDataTable, nameof(InputDataTable), true);
            metadata.AddRuntimeArgument(Result, nameof(Result), true);

            if (Columns == null)
            {
                if (Mode == DataRowEvaluationMode.Custom)
                    metadata.AddRuntimeArgument(Columns, typeof(IEnumerable<int>), nameof(Columns), true);

                return;
            }

            if (Columns.IsArgumentTypeAnyCompatible<IEnumerable<int>, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(Columns, Columns.ArgumentType, nameof(Columns), false);
            }
            else
            {
                metadata.AddValidationError(Resources.Validation_TypeErrorFormat("IEnumerable<string> or IEnumerable<int>", nameof(Columns)));
            }
        }

        protected override DataTable Execute(CodeActivityContext context)
        {
            var inputDt = InputDataTable.Get(context);

            bool predicate(object value) => value != null && value != DBNull.Value && !string.IsNullOrWhiteSpace(value.ToString());

            // default handler
            Func<DataRow, bool> handler = dr => dr.ItemArray.Any(predicate);

            if (Mode == DataRowEvaluationMode.Any)
            {
                handler = dr => dr.ItemArray.All(predicate);
            }
            else if (Mode == DataRowEvaluationMode.Custom)
            {
                handler = GetCustomModeHandler(context, inputDt, predicate);
            }

            var rows = inputDt.AsEnumerable().Where(handler);
            var dtResult = rows.Any() ? rows.CopyToDataTable() : inputDt.Clone();

            return dtResult;
        }

        private Func<DataRow, bool> GetCustomModeHandler(CodeActivityContext context, DataTable dt, Func<object, bool> predicate)
        {
            var indexes = DataTableUtil.IdentifyDataColumns(dt, Columns?.Get(context));

            IEnumerable<object> filter(DataRow dr) => dr.ItemArray.Where((_, index) => indexes.Contains(index));

            if (Operator == ConditionOperator.And)
                return (DataRow dr) => filter(dr).Any(predicate);

            return (DataRow dr) => filter(dr).All(predicate);
        }
    }
}