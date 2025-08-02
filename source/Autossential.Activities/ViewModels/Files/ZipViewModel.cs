using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.IO.Compression;
using System.Text;

namespace Autossential.Activities.ViewModels.Files
{
    public class ZipViewModel : BaseViewModel
    {
        public ZipViewModel(IDesignServices services) : base(services) { }

        public DesignInArgument ToCompress { get; set; }
        public DesignInArgument<string> ZipFilePath { get; set; }
        public DesignInArgument<Encoding> TextEncoding { get; set; }
        public DesignProperty<CompressionLevel> CompressionLevel { get; set; }
        public DesignInArgument<bool> ShortEntryNames { get; set; }
        public DesignOutArgument<int> FilesCount { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
            int orderIndex = 0;
            ToCompress.IsPrincipal = true;
            ToCompress.IsRequired = true;
            ToCompress.Category = Resources.Input_Category;
            ToCompress.DisplayName = Resources.Zip_ToCompress_DisplayName;
            ToCompress.Placeholder = Resources.Zip_ToCompress_Description;
            ToCompress.Tooltip = Resources.Zip_ToCompress_Description;
            ToCompress.OrderIndex = orderIndex++;

            ZipFilePath.IsPrincipal = true;
            ZipFilePath.IsRequired = true;
            ZipFilePath.Category = Resources.Input_Category;
            ZipFilePath.DisplayName = Resources.Zip_ZipFilePath_DisplayName;
            ZipFilePath.Placeholder = Resources.Zip_ZipFilePath_Description;
            ZipFilePath.Tooltip = Resources.Zip_ZipFilePath_Description;
            ZipFilePath.OrderIndex = orderIndex++;

            TextEncoding.IsPrincipal = false;
            TextEncoding.IsRequired = false;
            TextEncoding.Category = Resources.Options_Category;
            TextEncoding.DisplayName = Resources.Zip_TextEncoding_DisplayName;
            TextEncoding.Placeholder = Resources.Zip_TextEncoding_Description;
            TextEncoding.Tooltip = Resources.Zip_TextEncoding_Description;
            TextEncoding.OrderIndex = orderIndex++;

            CompressionLevel.IsPrincipal = false;
            CompressionLevel.IsRequired = false;
            CompressionLevel.Category = Resources.Options_Category;
            CompressionLevel.DisplayName = Resources.Zip_CompressionLevel_DisplayName;
            CompressionLevel.Placeholder = Resources.Zip_CompressionLevel_Description;
            CompressionLevel.Tooltip = Resources.Zip_CompressionLevel_Description;
            CompressionLevel.OrderIndex = orderIndex++;

            ShortEntryNames.IsPrincipal = false;
            ShortEntryNames.IsRequired = false;
            ShortEntryNames.Category = Resources.Options_Category;
            ShortEntryNames.DisplayName = Resources.Zip_ShortEntryNames_DisplayName;
            ShortEntryNames.Placeholder = Resources.Zip_ShortEntryNames_Description;
            ShortEntryNames.Tooltip = Resources.Zip_ShortEntryNames_Description;
            ShortEntryNames.OrderIndex = orderIndex++;

            FilesCount.IsPrincipal = false;
            FilesCount.IsRequired = false;
            FilesCount.Category = Resources.Output_Category;
            FilesCount.DisplayName = Resources.Zip_FilesCount_DisplayName;
            FilesCount.Placeholder = Resources.Zip_FilesCount_Description;
            FilesCount.Tooltip = Resources.Zip_FilesCount_Description;
            FilesCount.OrderIndex = orderIndex++;
        }
    }
}
