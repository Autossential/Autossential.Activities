using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels
{
    internal class PromoteHeadersViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInOutArgument<DataTable> DataTable { get; set; }
        public DesignInArgument<bool> AutoRename { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            DataTable.IsPrincipal = true;
            AddWidget(AutoRename, ViewModelWidgetType.Toggle);
        }
    }
}
