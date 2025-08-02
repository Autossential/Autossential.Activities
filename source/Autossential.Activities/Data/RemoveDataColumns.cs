using Autossential.Activities.Properties;
using Autossential.Shared;
using Autossential.Shared.Utils;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Autossential.Activities
{
    public class RemoveDataColumns : CodeActivity
    {
        public InOutArgument<DataTable> ReferenceDataTable { get; set; }

        public InArgument Columns { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(ReferenceDataTable, nameof(ReferenceDataTable), true);

            if (Columns == null)
            {
                metadata.AddValidationError(ResourcesFn.Validation_ValueErrorFormat(nameof(Columns)));
            }
            else if (Columns.IsArgumentTypeAnyCompatible<IEnumerable<int>, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(Columns, Columns.ArgumentType, nameof(Columns), true);
            }
            else
            {
                metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("IEnumerable<string> or IEnumerable<int>", nameof(Columns)));
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var dt = ReferenceDataTable.Get(context);
            foreach (var colIndex in DataTableUtil.IdentifyDataColumns(dt, Columns.Get(context)).OrderByDescending(v => v).ToArray())
                dt.Columns.RemoveAt(colIndex);

            ReferenceDataTable.Set(context, dt);
        }
    }
}