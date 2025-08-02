using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Programming
{
    public class IncrementViewModel : BaseViewModel
    {
        public IncrementViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<int> Value { get; set; }
        public DesignInOutArgument<int> Variable { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Variable.IsRequired = true;
            Variable.IsPrincipal = true;
            Variable.Category = Resources.Input_Category;
            Variable.DisplayName = Resources.Increment_Variable_DisplayName;
            Variable.Tooltip = Resources.Increment_Variable_Description;
            Variable.OrderIndex = orderIndex++;

            Value.IsRequired = true;
            Value.IsPrincipal = false;
            Value.Category = Resources.Input_Category;
            Value.DisplayName = Resources.Increment_Value_DisplayName;
            Value.Tooltip = Resources.Increment_Value_Description;
            Value.OrderIndex = orderIndex++;
        }
    }
}
