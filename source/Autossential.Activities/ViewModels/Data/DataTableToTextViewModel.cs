using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System.Activities.DesignViewModels;
using System.Data;

namespace Autossential.Activities.ViewModels.Data
{
    public class DataTableToTextViewModel : BaseViewModel
    {
        public DataTableToTextViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<DataTable> InputDataTable { get; set; }
        public DesignProperty<TextFormat> TextFormat { get; set; }
        public DesignInArgument<string> DateTimeFormat { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            InputDataTable.IsPrincipal = true;
            InputDataTable.IsRequired = true;
            InputDataTable.Category = Resources.Input_Category;
            InputDataTable.DisplayName = Resources.DataTableToText_InputDataTable_DisplayName;
            InputDataTable.Placeholder = Resources.DataTableToText_InputDataTable_Description;
            InputDataTable.Tooltip = Resources.DataTableToText_InputDataTable_Description;
            InputDataTable.OrderIndex = orderIndex++;

            TextFormat.IsPrincipal = true;
            TextFormat.IsRequired = true;
            TextFormat.Category = Resources.Input_Category;
            TextFormat.DisplayName = Resources.DataTableToText_TextFormat_DisplayName;
            TextFormat.Placeholder = Resources.DataTableToText_TextFormat_Description;
            TextFormat.Tooltip = Resources.DataTableToText_TextFormat_Description;
            TextFormat.OrderIndex = orderIndex++;

            DateTimeFormat.IsPrincipal = false;
            DateTimeFormat.IsRequired = false;
            DateTimeFormat.Category = Resources.Input_Category;
            DateTimeFormat.DisplayName = Resources.DataTableToText_DateTimeFormat_DisplayName;
            DateTimeFormat.Placeholder = Resources.DataTableToText_DateTimeFormat_Description;
            DateTimeFormat.Tooltip = Resources.DataTableToText_DateTimeFormat_Description;
            DateTimeFormat.OrderIndex = orderIndex++;
        }
    }
}