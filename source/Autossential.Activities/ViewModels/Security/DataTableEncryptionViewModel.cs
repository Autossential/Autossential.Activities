using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System.Activities.DesignViewModels;
using System.Data;
using System.Text;

namespace Autossential.Activities.ViewModels.Security
{
    public class DataTableEncryptionViewModel : BaseViewModel
    {
        public DataTableEncryptionViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument Columns { get; set; }
        public DesignInArgument<string> Sort { get; set; }
        public DesignProperty<bool> ParallelProcessing { get; set; }
        public DesignInArgument Key { get; set; }
        public DesignInArgument<DataTable> Input { get; set; }
        public DesignInArgument<Encoding> TextEncoding { get; set; }
        public DesignProperty<CryptoActions> Action { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Input.IsRequired = true;
            Input.IsPrincipal = true;
            Input.Category = Resources.Input_Category;
            Input.DisplayName = Resources.DataTableEncryption_Input_DisplayName;
            Input.Tooltip = Resources.DataTableEncryption_Input_Description;
            Input.OrderIndex = orderIndex++;

            Key.IsRequired = true;
            Key.IsPrincipal = true;
            Key.Category = Resources.Input_Category;
            Key.DisplayName = Resources.DataTableEncryption_Key_DisplayName;
            Key.Tooltip = Resources.DataTableEncryption_Key_Description;
            Key.OrderIndex = orderIndex++;

            Action.IsRequired = true;
            Action.IsPrincipal = true;
            Action.Category = Resources.Input_Category;
            Action.DisplayName = Resources.DataTableEncryption_Action_DisplayName;
            Action.Tooltip = Resources.DataTableEncryption_Action_Description;
            Action.OrderIndex = orderIndex++;

            Columns.IsRequired = false;
            Columns.IsPrincipal = false;
            Columns.Category = Resources.Options_Category;
            Columns.DisplayName = Resources.DataTableEncryption_Columns_DisplayName;
            Columns.Tooltip = Resources.DataTableEncryption_Columns_Description;
            Columns.OrderIndex = orderIndex++;

            Sort.IsRequired = false;
            Sort.IsPrincipal = false;
            Sort.Category = Resources.Options_Category;
            Sort.DisplayName = Resources.DataTableEncryption_Sort_DisplayName;
            Sort.Tooltip = Resources.DataTableEncryption_Sort_Description;
            Sort.OrderIndex = orderIndex++;

            ParallelProcessing.IsRequired = false;
            ParallelProcessing.IsPrincipal = false;
            ParallelProcessing.Category = Resources.Options_Category;
            ParallelProcessing.DisplayName = Resources.DataTableEncryption_ParallelProcessing_DisplayName;
            ParallelProcessing.Tooltip = Resources.DataTableEncryption_ParallelProcessing_Description;
            ParallelProcessing.OrderIndex = orderIndex++;

            TextEncoding.IsRequired = false;
            TextEncoding.IsPrincipal = false;
            TextEncoding.Category = Resources.Options_Category;
            TextEncoding.DisplayName = Resources.DataTableEncryption_TextEncoding_DisplayName;
            TextEncoding.Tooltip = Resources.DataTableEncryption_TextEncoding_Description;
            TextEncoding.OrderIndex = orderIndex++;
        }
    }
}
