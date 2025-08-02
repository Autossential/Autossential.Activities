using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Files
{
    public class WaitFileViewModel : BaseViewModel
    {
        public WaitFileViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> FilePath { get; set; }
        public DesignInArgument<int> Timeout { get; set; }
        public DesignInArgument<bool> WaitForExist { get; set; }
        public DesignInArgument<int> Interval { get; set; }
        public DesignOutArgument<System.IO.FileInfo> Result { get; set; }

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

            Timeout.IsPrincipal = false;
            Timeout.IsRequired = false;
            Timeout.Category = Resources.Options_Category;
            Timeout.DisplayName = Resources.WaitFile_Timeout_DisplayName;
            Timeout.Placeholder = Resources.WaitFile_Timeout_Description;
            Timeout.Tooltip = Resources.WaitFile_Timeout_Description;
            Timeout.OrderIndex = orderIndex++;

            WaitForExist.IsPrincipal = false;
            WaitForExist.IsRequired = false;
            WaitForExist.Category = Resources.Options_Category;
            WaitForExist.DisplayName = Resources.WaitFile_WaitForExist_DisplayName;
            WaitForExist.Placeholder = Resources.WaitFile_WaitForExist_Description;
            WaitForExist.Tooltip = Resources.WaitFile_WaitForExist_Description;
            WaitForExist.OrderIndex = orderIndex++;

            Interval.IsPrincipal = false;
            Interval.IsRequired = false;
            Interval.Category = Resources.Options_Category;
            Interval.DisplayName = Resources.WaitFile_Interval_DisplayName;
            Interval.Placeholder = Resources.WaitFile_Interval_Description;
            Interval.Tooltip = Resources.WaitFile_Interval_Description;
            Interval.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Output_Category;
            Result.DisplayName = Resources.WaitFile_Result_DisplayName;
            Result.Placeholder = Resources.WaitFile_Result_Description;
            Result.Tooltip = Resources.WaitFile_Result_Description;
            Result.OrderIndex = orderIndex++;
        }
    }
}