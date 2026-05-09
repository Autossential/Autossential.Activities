using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.IO.Compression;
using System.Text;

namespace Autossential.Activities.ViewModels
{
    internal class ZipViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> ZipFilePath { get; set; }
        public DesignInArgument<IReadOnlyList<string>> ToCompress { get; set; }
        public DesignInArgument<Encoding> TextEncoding { get; set; }
        public DesignProperty<CompressionLevel> CompressionLevel { get; set; }
        public DesignProperty<Zip.ZipEntryStructure> EntryStructure { get; set; }
        public DesignOutArgument<FileInfo> CompressedFile { get; set; }
        public DesignOutArgument<int> FilesCount { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;

            ToCompress.IsPrincipal = true;
            ToCompress.OrderIndex = orderIndex++;

            ZipFilePath.IsPrincipal = true;
            ZipFilePath.OrderIndex = orderIndex++;

#if WINDOWS
            if (IsWidgetSupported(ViewModelWidgetType.ActionButton))
            {
                ZipFilePath.AddFileDialogMenuAction(false, "All files (*.*)|*.*");
            }
#endif
        }
    }
}