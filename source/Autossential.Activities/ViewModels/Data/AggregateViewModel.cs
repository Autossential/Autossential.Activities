using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    public class AggregateViewModel : DesignPropertiesViewModel
    {
        public AggregateViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<DataTable> InputDataTable { get; set; }

        public DesignProperty<AggregateFunction> Function { get; set; }

        public DesignInArgument Columns { get; set; }

        public DesignOutArgument<object[]> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            InputDataTable.IsPrincipal = true;
            InputDataTable.IsRequired = true;
            InputDataTable.Category = Resources.Input_Category;
            InputDataTable.DisplayName = Resources.Aggregate_InputDataTable_DisplayName;
            InputDataTable.Placeholder = Resources.Aggregate_InputDataTable_Description;
            InputDataTable.Tooltip = Resources.Aggregate_InputDataTable_Description;
            InputDataTable.OrderIndex = orderIndex++;

            Function.IsPrincipal = true;
            Function.IsRequired = true;
            Function.Category = Resources.Input_Category;
            Function.DisplayName = Resources.Aggregate_Function_DisplayName;
            Function.Placeholder = Resources.Aggregate_Function_Description;
            Function.Tooltip = Resources.Aggregate_Function_Description;
            Function.OrderIndex = orderIndex++;

            Columns.IsPrincipal = false;
            Columns.IsRequired = false;
            Columns.Category = Resources.Input_Category;
            Columns.DisplayName = Resources.Aggregate_Columns_DisplayName;
            Columns.Placeholder = Resources.Aggregate_Columns_Description;
            Columns.Tooltip = Resources.Aggregate_Columns_Description;
            Columns.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Input_Category;
            Result.DisplayName = Resources.Aggregate_Result_DisplayName;
            Result.Placeholder = Resources.Aggregate_Result_Description;
            Result.Tooltip = Resources.Aggregate_Result_Description;
            Result.OrderIndex = orderIndex++;
        }
    }
}
