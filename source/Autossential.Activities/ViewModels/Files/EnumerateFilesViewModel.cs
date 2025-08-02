using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Collections.Generic;
using System.IO;

namespace Autossential.Activities.ViewModels.Files
{
    public class EnumerateFilesViewModel : BaseViewModel
    {
        public EnumerateFilesViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument DirectoryPath { get; set; }
        public DesignInArgument SearchPattern { get; set; }
        public DesignProperty<SearchOption> SearchOption { get; set; }
        public DesignProperty<FileAttributes> Exclusions { get; set; }
        public DesignInArgument<bool> FullPathMode { get; set; }
        public DesignOutArgument<IEnumerable<string>> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            DirectoryPath.IsPrincipal = true;
            DirectoryPath.IsRequired = true;
            DirectoryPath.Category = Resources.Input_Category;
            DirectoryPath.DisplayName = Resources.EnumerateFiles_DirectoryPath_DisplayName;
            DirectoryPath.Placeholder = Resources.EnumerateFiles_DirectoryPath_Description;
            DirectoryPath.Tooltip = Resources.EnumerateFiles_DirectoryPath_Description;
            DirectoryPath.OrderIndex = orderIndex++;

            SearchPattern.IsPrincipal = false;
            SearchPattern.IsRequired = false;
            SearchPattern.Category = Resources.Input_Category;
            SearchPattern.DisplayName = Resources.EnumerateFiles_SearchPattern_DisplayName;
            SearchPattern.Placeholder = Resources.Common_SearchPattern_Description;
            SearchPattern.Tooltip = Resources.Common_SearchPattern_Description;
            SearchPattern.OrderIndex = orderIndex++;

            SearchOption.IsPrincipal = false;
            SearchOption.IsRequired = false;
            SearchOption.Category = Resources.Options_Category;
            SearchOption.DisplayName = Resources.EnumerateFiles_SearchOption_DisplayName;
            SearchOption.Placeholder = Resources.EnumerateFiles_SearchOption_Description;
            SearchOption.Tooltip = Resources.EnumerateFiles_SearchOption_Description;
            SearchOption.OrderIndex = orderIndex++;

            Exclusions.IsPrincipal = false;
            Exclusions.IsRequired = false;
            Exclusions.Category = Resources.Options_Category;
            Exclusions.DisplayName = Resources.EnumerateFiles_Exclusions_DisplayName;
            Exclusions.Placeholder = Resources.EnumerateFiles_Exclusions_Description;
            Exclusions.Tooltip = Resources.EnumerateFiles_Exclusions_Description;
            Exclusions.OrderIndex = orderIndex++;

            FullPathMode.IsPrincipal = false;
            FullPathMode.IsRequired = false;
            FullPathMode.Category = Resources.Options_Category;
            FullPathMode.DisplayName = Resources.Common_FullPathMode_DisplayName;
            FullPathMode.Placeholder = Resources.Common_FullPathMode_Description;
            FullPathMode.Tooltip = Resources.Common_FullPathMode_Description;
            FullPathMode.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Output_Category;
            Result.DisplayName = Resources.EnumerateFiles_Result_DisplayName;
            Result.Placeholder = Resources.EnumerateFiles_Result_Description;
            Result.Tooltip = Resources.EnumerateFiles_Result_Description;
            Result.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.Toggle))
            {
                FullPathMode.Widget = new DefaultWidget
                {
                    Type = ViewModelWidgetType.Toggle
                };
            }
        }
    }
}