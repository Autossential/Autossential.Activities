using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Workflow
{
    internal class ExitViewModel : BaseViewModel
    {
        public ExitViewModel(IDesignServices services) : base(services) { }

        public DesignInArgument<bool> Condition { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            Condition.IsPrincipal = false;
            Condition.IsRequired = false;
            Condition.Category = Resources.Input_Category;
            Condition.DisplayName = Resources.Exit_Condition_DisplayName;
            Condition.Placeholder = Resources.Exit_Condition_Description;
            Condition.Tooltip = Resources.Exit_Condition_Description;
            Condition.OrderIndex = orderIndex++;
        }
    }
}
