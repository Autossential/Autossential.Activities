using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    public class IsTrueViewModel : DesignPropertiesViewModel
    {
        public IsTrueViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<bool> Value { get; set; }
        public DesignOutArgument<bool> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Value.IsRequired = true;
            Value.IsPrincipal = true;
            Value.Category = Resources.Input_Category;
            Value.Placeholder = Resources.IsTrue_Value_Description;
            Value.Tooltip = Resources.IsTrue_Value_Description;
            Value.DisplayName = Resources.IsTrue_Value_DisplayName;
            Value.OrderIndex = orderIndex++;

            Result.IsRequired = false;
            Result.IsPrincipal = false;
            Result.Category = Resources.Output_Category;
            Result.Placeholder = Resources.IsTrue_Result_Description;
            Result.Tooltip = Resources.IsTrue_Result_Description;
            Result.DisplayName = Resources.IsTrue_Result_DisplayName;
            Result.OrderIndex = orderIndex++;
        }
    }
}