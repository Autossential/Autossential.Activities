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
    public sealed class RemoveEmptyRows : CodeActivity
    {
        public InOutArgument<DataTable> DataTable { get; set; }

        public DataRowEvaluationMode Mode { get; set; }
        public InArgument Columns { get; set; }
        public ConditionOperator Operator { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(DataTable, nameof(DataTable), true);

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
                metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("IEnumerable<string> or IEnumerable<int>", nameof(Columns)));
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var inputDt = DataTable.Get(context);

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
            var outputDt = rows.Any() ? rows.CopyToDataTable() : inputDt.Clone();

            DataTable.Set(context, outputDt);
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