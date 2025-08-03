using Autossential.Activities.Properties;
using Autossential.Core.Enums;
using System;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.IO;

namespace Autossential.Activities.ViewModels.Files
{
    internal class WaitDynamicFileViewModel : BaseViewModel
    {
        public WaitDynamicFileViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> DirectoryPath { get; set; }
        public DesignInArgument<string> SearchPattern { get; set; }
        public DesignInArgument<double> TimeoutSeconds { get; set; }
        public DesignInArgument<DateTime?> FromDateTime { get; set; }
        public DesignInArgument<double> IntervalSeconds { get; set; }
        public DesignInArgument<bool> FullPathMode { get; set; }
        public DesignOutArgument<FileInfo> Result { get; set; }

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

            TimeoutSeconds.IsPrincipal = false;
            TimeoutSeconds.IsRequired = false;
            TimeoutSeconds.Category = Resources.Options_Category;
            TimeoutSeconds.DisplayName = Resources.Common_TimeoutSeconds_DisplayName;
            TimeoutSeconds.Placeholder = Resources.Common_TimeoutSeconds_Description;
            TimeoutSeconds.Tooltip = Resources.Common_TimeoutSeconds_Description;
            TimeoutSeconds.OrderIndex = orderIndex++;

            FromDateTime.IsPrincipal = false;
            FromDateTime.IsRequired = false;
            FromDateTime.Category = Resources.Options_Category;
            FromDateTime.DisplayName = Resources.WaitDynamicFile_FromDateTime_DisplayName;
            FromDateTime.Placeholder = Resources.WaitDynamicFile_FromDateTime_Description;
            FromDateTime.Tooltip = Resources.WaitDynamicFile_FromDateTime_Description;
            FromDateTime.OrderIndex = orderIndex++;

            IntervalSeconds.IsPrincipal = false;
            IntervalSeconds.IsRequired = false;
            IntervalSeconds.Category = Resources.Options_Category;
            IntervalSeconds.DisplayName = Resources.Common_IntervalSeconds_DisplayName;
            IntervalSeconds.Placeholder = Resources.Common_IntervalSeconds_Description;
            IntervalSeconds.Tooltip = Resources.Common_IntervalSeconds_Description;
            IntervalSeconds.OrderIndex = orderIndex++;

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
            Result.DisplayName = Resources.WaitDynamicFile_Result_DisplayName;
            Result.Placeholder = Resources.WaitDynamicFile_Result_Description;
            Result.Tooltip = Resources.WaitDynamicFile_Result_Description;
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