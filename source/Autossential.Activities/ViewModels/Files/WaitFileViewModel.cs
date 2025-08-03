using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.IO;

namespace Autossential.Activities.ViewModels.Files
{
    internal class WaitFileViewModel : BaseViewModel
    {
        public WaitFileViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> FilePath { get; set; }
        public DesignInArgument<double> TimeoutSeconds { get; set; }
        public DesignInArgument<bool> WaitForExist { get; set; }
        public DesignInArgument<double> IntervalSeconds { get; set; }
        public DesignOutArgument<FileInfo> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            FilePath.IsPrincipal = true;
            FilePath.IsRequired = true;
            FilePath.Category = Resources.Input_Category;
            FilePath.DisplayName = Resources.WaitFile_FilePath_DisplayName;
            FilePath.Placeholder = Resources.WaitFile_FilePath_Description;
            FilePath.Tooltip = Resources.WaitFile_FilePath_Description;
            FilePath.OrderIndex = orderIndex++;

            TimeoutSeconds.IsPrincipal = false;
            TimeoutSeconds.IsRequired = false;
            TimeoutSeconds.Category = Resources.Options_Category;
            TimeoutSeconds.DisplayName = Resources.Common_TimeoutSeconds_DisplayName;
            TimeoutSeconds.Placeholder = Resources.Common_TimeoutSeconds_Description;
            TimeoutSeconds.Tooltip = Resources.Common_TimeoutSeconds_Description;
            TimeoutSeconds.OrderIndex = orderIndex++;

            WaitForExist.IsPrincipal = false;
            WaitForExist.IsRequired = false;
            WaitForExist.Category = Resources.Options_Category;
            WaitForExist.DisplayName = Resources.WaitFile_WaitForExist_DisplayName;
            WaitForExist.Placeholder = Resources.WaitFile_WaitForExist_Description;
            WaitForExist.Tooltip = Resources.WaitFile_WaitForExist_Description;
            WaitForExist.OrderIndex = orderIndex++;

            IntervalSeconds.IsPrincipal = false;
            IntervalSeconds.IsRequired = false;
            IntervalSeconds.Category = Resources.Options_Category;
            IntervalSeconds.DisplayName = Resources.Common_IntervalSeconds_DisplayName;
            IntervalSeconds.Placeholder = Resources.Common_IntervalSeconds_Description;
            IntervalSeconds.Tooltip = Resources.Common_IntervalSeconds_Description;
            IntervalSeconds.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Output_Category;
            Result.DisplayName = Resources.WaitFile_Result_DisplayName;
            Result.Placeholder = Resources.WaitFile_Result_Description;
            Result.Tooltip = Resources.WaitFile_Result_Description;
            Result.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.Toggle))
            {
                WaitForExist.Widget = new DefaultWidget
                {
                    Type = ViewModelWidgetType.Toggle
                };
            }
        }
    }
}