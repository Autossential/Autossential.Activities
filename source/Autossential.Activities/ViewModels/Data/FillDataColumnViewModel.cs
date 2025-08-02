using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    public class FillDataColumnViewModel : BaseViewModel
    {
        public FillDataColumnViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument<DataTable> ReferenceDataTable { get; set; }
        public DesignInArgument Column { get; set; }
        public DesignInArgument Value { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            ReferenceDataTable.IsPrincipal = true;
            ReferenceDataTable.IsRequired = true;
            ReferenceDataTable.Category = Resources.InputOutput_Category;
            ReferenceDataTable.DisplayName = Resources.FillDataColumn_ReferenceDataTable_DisplayName;
            ReferenceDataTable.Placeholder = Resources.FillDataColumn_ReferenceDataTable_Description;
            ReferenceDataTable.OrderIndex = orderIndex++;

            Column.IsPrincipal = true;
            Column.IsRequired = true;
            Column.Category = Resources.Input_Category;
            Column.DisplayName = Resources.FillDataColumn_Column_DisplayName;
            Column.Placeholder = Resources.FillDataColumn_Column_Description;
            Column.OrderIndex = orderIndex++;

            Value.IsPrincipal = true;
            Value.IsRequired = false;
            Value.Category = Resources.Input_Category;
            Value.DisplayName = Resources.FillDataColumn_Value_DisplayName;
            Value.Placeholder = Resources.FillDataColumn_Value_Description;
            Value.OrderIndex = orderIndex++;
        }
    }
}