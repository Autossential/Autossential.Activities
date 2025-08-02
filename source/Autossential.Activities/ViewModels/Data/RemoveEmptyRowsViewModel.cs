using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    public class RemoveEmptyRowsViewModel : DesignPropertiesViewModel
    {
        public RemoveEmptyRowsViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument<DataTable> DataTable { get; set; }
        public DesignProperty<DataRowEvaluationMode> Mode { get; set; }
        public DesignInArgument Columns { get; set; }
        public DesignProperty<ConditionOperator> Operator { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            DataTable.IsPrincipal = true;
            DataTable.IsRequired = true;
            DataTable.Category = Resources.Input_Category;
            DataTable.DisplayName = Resources.RemoveEmptyRows_InputDataTable_DisplayName;
            DataTable.Placeholder = Resources.RemoveEmptyRows_InputDataTable_Description;
            DataTable.OrderIndex = orderIndex++;

            Mode.IsPrincipal = true;
            Mode.IsRequired = true;
            Mode.Category = Resources.Input_Category;
            Mode.DisplayName = Resources.RemoveEmptyRows_Mode_DisplayName;
            Mode.Placeholder = Resources.RemoveEmptyRows_Mode_Description;
            Mode.OrderIndex = orderIndex++;

            Columns.IsPrincipal = true;
            Columns.IsRequired = false;
            Columns.IsVisible = false;
            Columns.Category = Resources.Input_Category;
            Columns.DisplayName = Resources.RemoveEmptyRows_Columns_DisplayName;
            Columns.Placeholder = Resources.RemoveEmptyRows_Columns_Description;
            Columns.OrderIndex = orderIndex++;

            Operator.IsPrincipal = true;
            Operator.IsRequired = false;
            Operator.IsVisible = false;
            Operator.Category = Resources.Input_Category;
            Operator.DisplayName = Resources.RemoveEmptyRows_Operator_DisplayName;
            Operator.Placeholder = Resources.RemoveEmptyRows_Operator_Description;
            Operator.OrderIndex = orderIndex++;
        }

        protected override void InitializeRules()
        {
            base.InitializeRules();
            Rule(nameof(Mode), ModeChange, true);
        }

        private void ModeChange()
        {
            if (Mode.Value == DataRowEvaluationMode.Custom)
            {
                Columns.IsVisible = true;
                Operator.IsVisible = true;
            }
            else
            {
                Columns.IsVisible = false;
                Operator.IsVisible = false;
            }
        }
    }
}
