using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    internal class TransposeDataViewModel : BaseViewModel
    {
        public TransposeDataViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument<DataTable> DataTable { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            DataTable.IsPrincipal = true;
            DataTable.IsRequired = true;
            DataTable.Category = Resources.Input_Category;
            DataTable.DisplayName = Resources.TransposeData_InputDataTable_DisplayName;
            DataTable.Placeholder = Resources.TransposeData_InputDataTable_Description;
            DataTable.OrderIndex = orderIndex++;
        }
    }
}