using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    public class PromoteHeadersViewModel : BaseViewModel
    {
        public PromoteHeadersViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument<DataTable> DataTable { get; set; }
        public DesignInArgument<bool> AutoRename { get; set; }
        public DesignInArgument<string> EmptyColumnName { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            DataTable.IsPrincipal = true;
            DataTable.IsRequired = true;
            DataTable.Category = Resources.Input_Category;
            DataTable.DisplayName = Resources.PromoteHeaders_InputDataTable_DisplayName;
            DataTable.Placeholder = Resources.PromoteHeaders_InputDataTable_Description;
            DataTable.OrderIndex = orderIndex++;

            AutoRename.IsPrincipal = false;
            AutoRename.IsRequired = false;
            AutoRename.Category = Resources.Options_Category;
            AutoRename.DisplayName = Resources.PromoteHeaders_AutoRename_DisplayName;
            AutoRename.Placeholder = Resources.PromoteHeaders_AutoRename_Description;
            AutoRename.OrderIndex = orderIndex++;

            EmptyColumnName.IsPrincipal = false;
            EmptyColumnName.IsRequired = false;
            EmptyColumnName.Category = Resources.Options_Category;
            EmptyColumnName.DisplayName = Resources.PromoteHeaders_EmptyColumnName_DisplayName;
            EmptyColumnName.Placeholder = Resources.PromoteHeaders_EmptyColumnName_Description;
            EmptyColumnName.OrderIndex = orderIndex++;
        }
    }
}