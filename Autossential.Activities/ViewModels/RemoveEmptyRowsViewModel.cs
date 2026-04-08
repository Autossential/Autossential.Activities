using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Autossential.Activities.ViewModels
{
    internal class RemoveEmptyRowsViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInOutArgument<DataTable> DataTable { get; set; }
        public DesignInArgument<IReadOnlyList<string>> ColumnNames { get; set; }
        public DesignInArgument<IReadOnlyList<int>> ColumnIndexes { get; set; }
        public DesignProperty<RemoveEmptyRows.MatchMode> MatchingMode { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            DataTable.IsPrincipal = true;

            AddWidget(MatchingMode, ViewModelWidgetType.RadioGroup);
        }
    }
}
