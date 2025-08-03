using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels.Files
{
    internal class UnzipViewModel : BaseViewModel
    {
        public UnzipViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> ZipFilePath { get; set; }
        public DesignInArgument<string> ExtractTo { get; set; }
        public DesignInArgument<bool> Overwrite { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            ZipFilePath.IsPrincipal = true;
            ZipFilePath.IsRequired = true;
            ZipFilePath.Category = Resources.Input_Category;
            ZipFilePath.DisplayName = Resources.Unzip_ZipFilePath_DisplayName;
            ZipFilePath.Placeholder = Resources.Unzip_ZipFilePath_Description;
            ZipFilePath.Tooltip = Resources.Unzip_ZipFilePath_Description;
            ZipFilePath.OrderIndex = orderIndex++;

            ExtractTo.IsPrincipal = true;
            ExtractTo.IsRequired = true;
            ExtractTo.Category = Resources.Input_Category;
            ExtractTo.DisplayName = Resources.Unzip_ExtractTo_DisplayName;
            ExtractTo.Placeholder = Resources.Unzip_ExtractTo_Description;
            ExtractTo.Tooltip = Resources.Unzip_ExtractTo_Description;
            ExtractTo.OrderIndex = orderIndex++;

            Overwrite.IsPrincipal = false;
            Overwrite.IsRequired = false;
            Overwrite.Category = Resources.Options_Category;
            Overwrite.DisplayName = Resources.Unzip_Overwrite_DisplayName;
            Overwrite.Placeholder = Resources.Unzip_Overwrite_Description;
            Overwrite.Tooltip = Resources.Unzip_Overwrite_Description;
            Overwrite.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.Toggle))
            {
                Overwrite.Widget = new DefaultWidget
                {
                    Type = ViewModelWidgetType.Toggle
                };
            }
        }
    }
}