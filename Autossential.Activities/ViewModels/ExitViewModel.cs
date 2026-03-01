
using Autossential.Activities.Base;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class ExitViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<bool> Condition { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
        }
    }
}
