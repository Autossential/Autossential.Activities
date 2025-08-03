using Autossential.Activities.Properties;
using Autossential.Core.Models;
using System;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.IO;

namespace Autossential.Activities.ViewModels.Files
{
    internal class CleanUpFolderViewModel : BaseViewModel
    {
        public CleanUpFolderViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> Folder { get; set; }
        public DesignInArgument SearchPattern { get; set; }
        public DesignInArgument<DateTime?> LastWriteTime { get; set; }
        public DesignInArgument<bool> DeleteEmptyFolders { get; set; }
        public DesignProperty<SearchOption> SearchOption { get; set; }
        public DesignInArgument<bool> FullPathMode { get; set; }
        public DesignOutArgument<CleanUpFolderResult> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            Folder.IsPrincipal = true;
            Folder.IsRequired = true;
            Folder.Category = Resources.Input_Category;
            Folder.DisplayName = Resources.CleanUpFolder_Folder_DisplayName;
            Folder.Placeholder = Resources.CleanUpFolder_Folder_Description;
            Folder.Tooltip = Resources.CleanUpFolder_Folder_Description;
            Folder.OrderIndex = orderIndex++;

            SearchPattern.IsPrincipal = false;
            SearchPattern.IsRequired = false;
            SearchPattern.Category = Resources.Input_Category;
            SearchPattern.DisplayName = Resources.CleanUpFolder_SearchPattern_DisplayName;
            SearchPattern.Placeholder = Resources.Common_SearchPattern_Description;
            SearchPattern.Tooltip = Resources.Common_SearchPattern_Description;
            SearchPattern.OrderIndex = orderIndex++;

            LastWriteTime.IsPrincipal = false;
            LastWriteTime.IsRequired = false;
            LastWriteTime.Category = Resources.Input_Category;
            LastWriteTime.DisplayName = Resources.CleanUpFolder_LastWriteTime_DisplayName;
            LastWriteTime.Placeholder = Resources.CleanUpFolder_LastWriteTime_Description;
            LastWriteTime.Tooltip = Resources.CleanUpFolder_LastWriteTime_Description;
            LastWriteTime.OrderIndex = orderIndex++;

            DeleteEmptyFolders.IsPrincipal = false;
            DeleteEmptyFolders.IsRequired = false;
            DeleteEmptyFolders.Category = Resources.Options_Category;
            DeleteEmptyFolders.DisplayName = Resources.CleanUpFolder_DeleteEmptyFolders_DisplayName;
            DeleteEmptyFolders.Placeholder = Resources.CleanUpFolder_DeleteEmptyFolders_Description;
            DeleteEmptyFolders.Tooltip = Resources.CleanUpFolder_DeleteEmptyFolders_Description;
            DeleteEmptyFolders.OrderIndex = orderIndex++;

            SearchOption.IsPrincipal = false;
            SearchOption.IsRequired = false;
            SearchOption.Category = Resources.Options_Category;
            SearchOption.DisplayName = Resources.CleanUpFolder_SearchOption_DisplayName;
            SearchOption.Placeholder = Resources.CleanUpFolder_SearchOption_Description;
            SearchOption.Tooltip = Resources.CleanUpFolder_SearchOption_Description;
            SearchOption.OrderIndex = orderIndex++;

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
            Result.DisplayName = Resources.CleanUpFolder_Result_DisplayName;
            Result.Placeholder = Resources.CleanUpFolder_Result_Description;
            Result.Tooltip = Resources.CleanUpFolder_Result_Description;
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