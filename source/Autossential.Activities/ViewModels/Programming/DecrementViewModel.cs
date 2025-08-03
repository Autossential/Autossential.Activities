using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Programming
{
    internal class DecrementViewModel : BaseViewModel
    {
        public DecrementViewModel(IDesignServices services) : base(services)
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
            Variable.DisplayName = Resources.Decrement_Variable_DisplayName;
            Variable.Tooltip = Resources.Decrement_Variable_Description;
            Variable.OrderIndex = orderIndex++;

            Value.IsRequired = true;
            Value.IsPrincipal = false;
            Value.Category = Resources.Input_Category;
            Value.DisplayName = Resources.Decrement_Value_DisplayName;
            Value.Tooltip = Resources.Decrement_Value_Description;
            Value.OrderIndex = orderIndex++;
        }
    }
}
