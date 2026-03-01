using Autossential.Activities.Base;
using System.Activities;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class IfActivityViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignProperty<ActivityFunc<bool>> Condition { get; set; }

        public DesignProperty<bool> CheckTrue { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            Condition.IsPrincipal = true;
        }
    }
}
