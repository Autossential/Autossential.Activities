using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System.Activities.DesignViewModels;
using System.Text;

namespace Autossential.Activities.ViewModels.Security
{
    internal class TextEncryptionViewModel : BaseViewModel
    {
        public TextEncryptionViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument Key { get; set; }
        public DesignInArgument<string> Input { get; set; }
        public DesignInArgument<Encoding> TextEncoding { get; set; }
        public DesignProperty<CryptoActions> Action { get; set; }

        public DesignOutArgument<string> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Input.IsRequired = true;
            Input.IsPrincipal = true;
            Input.Category = Resources.Input_Category;
            Input.DisplayName = Resources.TextEncryption_Input_DisplayName;
            Input.Placeholder = Resources.TextEncryption_Input_Description;
            Input.Tooltip = Resources.TextEncryption_Input_Description;
            Input.OrderIndex = orderIndex++;

            Key.IsRequired = true;
            Key.IsPrincipal = true;
            Key.Category = Resources.Input_Category;
            Key.DisplayName = Resources.TextEncryption_Key_DisplayName;
            Key.Placeholder = Resources.TextEncryption_Key_Description;
            Key.Tooltip = Resources.TextEncryption_Key_Description;
            Key.OrderIndex = orderIndex++;

            Action.IsRequired = true;
            Action.IsPrincipal = true;
            Action.Category = Resources.Input_Category;
            Action.DisplayName = Resources.TextEncryption_Action_DisplayName;
            Action.Placeholder = Resources.TextEncryption_Action_Description;
            Action.Tooltip = Resources.TextEncryption_Action_Description;
            Action.OrderIndex = orderIndex++;

            TextEncoding.IsRequired = false;
            TextEncoding.IsPrincipal = false;
            TextEncoding.Category = Resources.Options_Category;
            TextEncoding.DisplayName = Resources.TextEncryption_TextEncoding_DisplayName;
            TextEncoding.Placeholder = Resources.TextEncryption_TextEncoding_Description;
            TextEncoding.Tooltip = Resources.TextEncryption_TextEncoding_Description;
            TextEncoding.OrderIndex = orderIndex++;

            Result.IsRequired = false;
            Result.IsPrincipal = false;
            Result.Category = Resources.Options_Category;
            Result.DisplayName = Resources.TextEncryption_Result_DisplayName;
            Result.Placeholder = Resources.TextEncryption_Result_Description;
            Result.Tooltip = Resources.TextEncryption_Result_Description;
            Result.OrderIndex = orderIndex++;
        }
    }
}
