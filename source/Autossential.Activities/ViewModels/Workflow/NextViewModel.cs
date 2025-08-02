using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Workflow
{
    public class NextViewModel : BaseViewModel
    {
        public NextViewModel(IDesignServices services) : base(services) { }

        public DesignInArgument<bool> Condition { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            Condition.IsPrincipal = false;
            Condition.IsRequired = false;
            Condition.Category = Resources.Input_Category;
            Condition.DisplayName = Resources.Next_Condition_DisplayName;
            Condition.Placeholder = Resources.Next_Condition_Description;
            Condition.Tooltip = Resources.Next_Condition_Description;
            Condition.OrderIndex = orderIndex++;
        }
    }
}
