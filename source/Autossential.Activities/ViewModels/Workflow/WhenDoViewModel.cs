using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Workflow
{
    public class WhenDoViewModel : BaseViewModel
    {
        public WhenDoViewModel(IDesignServices services) : base(services) { }

        public DesignProperty<bool> Inverted { get; set; }
        public DesignProperty<bool> WithElse { get; set; }
        public DesignInArgument Condition { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            Condition.IsPrincipal = true;
            Condition.IsRequired = true;
            Condition.Category = Resources.Input_Category;
            Condition.DisplayName = Resources.CheckPoint_Expression_DisplayName;
            Condition.Placeholder = Resources.CheckPoint_Expression_Description;
            Condition.Tooltip = Resources.CheckPoint_Expression_Description;
            Condition.OrderIndex = orderIndex++;

            Inverted.IsPrincipal = false;
            Inverted.IsRequired = false;
            Inverted.Category = Resources.Options_Category;
            Inverted.DisplayName = Resources.WhenDo_Inverted_DisplayName;
            Inverted.Placeholder = Resources.WhenDo_Inverted_Description;
            Inverted.Tooltip = Resources.WhenDo_Inverted_Description;
            Inverted.OrderIndex = orderIndex++;

            WithElse.IsPrincipal = false;
            WithElse.IsRequired = false;
            WithElse.Category = Resources.Options_Category;
            WithElse.DisplayName = Resources.WhenDo_WithElse_DisplayName;
            WithElse.Placeholder = Resources.WhenDo_WithElse_Description;
            WithElse.Tooltip = Resources.WhenDo_WithElse_Description;
            WithElse.OrderIndex = orderIndex++;
        }
    }
}
