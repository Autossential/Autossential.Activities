using Autossential.Activities.Base;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class CultureScopeViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> CultureName { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            CultureName.IsPrincipal = true;
        }
    }
}