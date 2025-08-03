using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    internal class RemoveDataColumnsViewModel : BaseViewModel
    {
        public RemoveDataColumnsViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument<DataTable> ReferenceDataTable { get; set; }
        public DesignInArgument Columns { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            ReferenceDataTable.IsPrincipal = true;
            ReferenceDataTable.IsRequired = true;
            ReferenceDataTable.Category = Resources.InputOutput_Category;
            ReferenceDataTable.DisplayName = Resources.RemoveDataColumns_ReferenceDataTable_DisplayName;
            ReferenceDataTable.Placeholder = Resources.RemoveDataColumns_ReferenceDataTable_Description;
            ReferenceDataTable.OrderIndex = orderIndex++;

            Columns.IsPrincipal = true;
            Columns.IsRequired = true;
            Columns.Category = Resources.Input_Category;
            Columns.DisplayName = Resources.RemoveDataColumns_Columns_DisplayName;
            Columns.Placeholder = Resources.RemoveDataColumns_Columns_Description;
            Columns.OrderIndex = orderIndex++;
        }
    }
}