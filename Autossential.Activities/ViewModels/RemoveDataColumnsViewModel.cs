using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels
{
    internal class RemoveDataColumnsViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInOutArgument<DataTable> DataTable { get; set; }
        public DesignInArgument<IReadOnlyList<string>> ColumnNames { get; set; }
        public DesignInArgument<IReadOnlyList<int>> ColumnIndexes { get; set; }
        protected override void InitializeRules()
        {
            base.InitializeRules();

            DataTable.IsPrincipal = true;
            ColumnNames.IsPrincipal = true;
            ColumnIndexes.IsPrincipal = true;
        }
    }
}
