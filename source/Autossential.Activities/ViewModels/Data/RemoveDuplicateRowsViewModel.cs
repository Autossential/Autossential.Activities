using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    internal class RemoveDuplicateRowsViewModel : BaseViewModel
    {
        public RemoveDuplicateRowsViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument<DataTable> DataTable { get; set; }
        public DesignInArgument Columns { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            DataTable.IsPrincipal = true;
            DataTable.IsRequired = true;
            DataTable.Category = Resources.Input_Category;
            DataTable.DisplayName = Resources.RemoveDuplicateRows_InputDataTable_DisplayName;
            DataTable.Placeholder = Resources.RemoveDuplicateRows_InputDataTable_Description;
            DataTable.OrderIndex = orderIndex++;

            Columns.IsPrincipal = false;
            Columns.IsRequired = false;
            Columns.Category = Resources.Input_Category;
            Columns.DisplayName = Resources.RemoveDuplicateRows_Columns_DisplayName;
            Columns.Placeholder = Resources.RemoveDuplicateRows_Columns_Description;
            Columns.OrderIndex = orderIndex++;
        }
    }
}