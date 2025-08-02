using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Files
{
    public class WaitDynamicFileViewModel : BaseViewModel
    {
        public WaitDynamicFileViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> DirectoryPath { get; set; }
        public DesignInArgument<string> SearchPattern { get; set; }
        public DesignInArgument<int> Timeout { get; set; }
        public DesignInArgument<System.DateTime?> FromDateTime { get; set; }
        public DesignInArgument<int> Interval { get; set; }
        public DesignProperty<PatternSearchMode> SearchPatternMode { get; set; }
        public DesignOutArgument<System.IO.FileInfo> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            DirectoryPath.IsPrincipal = true;
            DirectoryPath.IsRequired = true;
            DirectoryPath.Category = Resources.Input_Category;
            DirectoryPath.DisplayName = Resources.WaitDynamicFile_DirectoryPath_DisplayName;
            DirectoryPath.Placeholder = Resources.WaitDynamicFile_DirectoryPath_Description;
            DirectoryPath.Tooltip = Resources.WaitDynamicFile_DirectoryPath_Description;
            DirectoryPath.OrderIndex = orderIndex++;

            SearchPattern.IsPrincipal = false;
            SearchPattern.IsRequired = false;
            SearchPattern.Category = Resources.Input_Category;
            SearchPattern.DisplayName = Resources.WaitDynamicFile_SearchPattern_DisplayName;
            SearchPattern.Placeholder = Resources.WaitDynamicFile_SearchPattern_Description;
            SearchPattern.Tooltip = Resources.WaitDynamicFile_SearchPattern_Description;
            SearchPattern.OrderIndex = orderIndex++;

            Timeout.IsPrincipal = false;
            Timeout.IsRequired = false;
            Timeout.Category = Resources.Options_Category;
            Timeout.DisplayName = Resources.WaitDynamicFile_Timeout_DisplayName;
            Timeout.Placeholder = Resources.WaitDynamicFile_Timeout_Description;
            Timeout.Tooltip = Resources.WaitDynamicFile_Timeout_Description;
            Timeout.OrderIndex = orderIndex++;

            FromDateTime.IsPrincipal = false;
            FromDateTime.IsRequired = false;
            FromDateTime.Category = Resources.Options_Category;
            FromDateTime.DisplayName = Resources.WaitDynamicFile_FromDateTime_DisplayName;
            FromDateTime.Placeholder = Resources.WaitDynamicFile_FromDateTime_Description;
            FromDateTime.Tooltip = Resources.WaitDynamicFile_FromDateTime_Description;
            FromDateTime.OrderIndex = orderIndex++;

            Interval.IsPrincipal = false;
            Interval.IsRequired = false;
            Interval.Category = Resources.Options_Category;
            Interval.DisplayName = Resources.WaitDynamicFile_Interval_DisplayName;
            Interval.Placeholder = Resources.WaitDynamicFile_Interval_Description;
            Interval.Tooltip = Resources.WaitDynamicFile_Interval_Description;
            Interval.OrderIndex = orderIndex++;

            SearchPatternMode.IsPrincipal = false;
            SearchPatternMode.IsRequired = false;
            SearchPatternMode.Category = Resources.Options_Category;
            SearchPatternMode.DisplayName = Resources.Common_FullPathMode_DisplayName;
            SearchPatternMode.Placeholder = Resources.Common_FullPathMode_Description;
            SearchPatternMode.Tooltip = Resources.Common_FullPathMode_Description;
            SearchPatternMode.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Output_Category;
            Result.DisplayName = Resources.WaitDynamicFile_Result_DisplayName;
            Result.Placeholder = Resources.WaitDynamicFile_Result_Description;
            Result.Tooltip = Resources.WaitDynamicFile_Result_Description;
            Result.OrderIndex = orderIndex++;
        }
    }
}