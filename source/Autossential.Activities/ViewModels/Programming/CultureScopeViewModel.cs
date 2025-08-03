using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Programming
{
    internal class CultureScopeViewModel : BaseViewModel
    {
        public CultureScopeViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> CultureName { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            CultureName.IsRequired = true;
            CultureName.IsPrincipal = true;
            CultureName.Category = Resources.Input_Category;
            CultureName.DisplayName = Resources.CultureScope_CultureName_DisplayName;
            CultureName.Tooltip = Resources.CultureScope_CultureName_Description;
        }
    }
}
