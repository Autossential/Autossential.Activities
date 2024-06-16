using Autossential.Activities.Properties;
using Autossential.Shared;
using Autossential.Shared.Utils;
using System;
using System.Activities;
using System.Data;

namespace Autossential.Activities
{
    public class FillDataColumn : CodeActivity
    {
        public InOutArgument<DataTable> ReferenceDataTable { get; set; }
        public InArgument Column { get; set; }
        public InArgument Value { get; set; }
        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(ReferenceDataTable, nameof(ReferenceDataTable), true);

            if (Value != null)
            {
                metadata.AddRuntimeArgument(Value, Value.ArgumentType, nameof(Value), true);
            }

            if (Column == null)
            {
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(Column)));
            }
            else if (Column.IsArgumentTypeAnyCompatible<int, string>())
            {
                metadata.AddRuntimeArgument(Column, Column.ArgumentType, nameof(Column), true);
            }
            else
            {
                metadata.AddValidationError(Resources.Validation_TypeErrorFormat("Int32 or String", nameof(Column)));
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var table = ReferenceDataTable.Get(context);
            var column = Column.Get(context);
            var value = Value?.Get(context);
            var index = DataTableUtil.IdentifyDataColumn(table, column, -1);

            if (index == -1)
            {
                if (column is int)
                    throw new ArgumentOutOfRangeException(Resources.FillDataTable_ErrorMsg_ColumnFormat(column));

                throw new ArgumentException(Resources.FillDataTable_ErrorMsg_ColumnFormat(column));
            }

            var col = table.Columns[index];
            foreach (DataRow row in table.Rows)
                row[col] = value;
        }
    }
}
