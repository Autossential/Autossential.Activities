using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Files
{
    public class ZipEntriesCountViewModel : BaseViewModel
    {
        public ZipEntriesCountViewModel(IDesignServices services) : base(services) { }

        public DesignInArgument<string> ZipFilePath { get; set; }
        public DesignOutArgument<int> EntriesCount { get; set; }
        public DesignOutArgument<int> FilesCount { get; set; }
        public DesignOutArgument<int> FoldersCount { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            ZipFilePath.IsPrincipal = true;
            ZipFilePath.IsRequired = true;
            ZipFilePath.Category = Resources.Input_Category;
            ZipFilePath.DisplayName = Resources.ZipEntriesCount_ZipFilePath_DisplayName;
            ZipFilePath.Placeholder = Resources.ZipEntriesCount_ZipFilePath_Description;
            ZipFilePath.Tooltip = Resources.ZipEntriesCount_ZipFilePath_Description;
            ZipFilePath.OrderIndex = orderIndex++;

            EntriesCount.IsPrincipal = false;
            EntriesCount.IsRequired = false;
            EntriesCount.Category = Resources.Output_Category;
            EntriesCount.DisplayName = Resources.ZipEntriesCount_EntriesCount_DisplayName;
            EntriesCount.Placeholder = Resources.ZipEntriesCount_EntriesCount_Description;
            EntriesCount.Tooltip = Resources.ZipEntriesCount_EntriesCount_Description;
            EntriesCount.OrderIndex = orderIndex++;

            FilesCount.IsPrincipal = false;
            FilesCount.IsRequired = false;
            FilesCount.Category = Resources.Output_Category;
            FilesCount.DisplayName = Resources.ZipEntriesCount_FilesCount_DisplayName;
            FilesCount.Placeholder = Resources.ZipEntriesCount_FilesCount_Description;
            FilesCount.Tooltip = Resources.ZipEntriesCount_FilesCount_Description;
            FilesCount.OrderIndex = orderIndex++;

            FoldersCount.IsPrincipal = false;
            FoldersCount.IsRequired = false;
            FoldersCount.Category = Resources.Output_Category;
            FoldersCount.DisplayName = Resources.ZipEntriesCount_FoldersCount_DisplayName;
            FoldersCount.Placeholder = Resources.ZipEntriesCount_FoldersCount_Description;
            FoldersCount.Tooltip = Resources.ZipEntriesCount_FoldersCount_Description;
            FoldersCount.OrderIndex = orderIndex++;
        }
    }
}
