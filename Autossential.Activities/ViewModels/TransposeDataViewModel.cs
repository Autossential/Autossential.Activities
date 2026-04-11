using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels
{
    internal class TransposeDataViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInOutArgument<DataTable> DataTable { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            DataTable.IsPrincipal = true;
        }
    }
}
