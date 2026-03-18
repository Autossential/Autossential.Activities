using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.IO.Compression;
using System.Text;

namespace Autossential.Activities.ViewModels
{
    internal class ZipViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> ZipFilePath { get; set; }
        public DesignInArgument<IEnumerable<string>> ToCompress { get; set; }
        public DesignInArgument<Encoding> TextEncoding { get; set; }
        public DesignProperty<CompressionLevel> CompressionLevel { get; set; }
        public DesignInArgument<bool> FullEntryNames { get; set; }
        public DesignOutArgument<FileInfo> CompressedFile { get; set; }
        public DesignOutArgument<int> FilesCount { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;

            ToCompress.IsPrincipal = true;
            ToCompress.IsRequired = true;
            ToCompress.OrderIndex = orderIndex++;

            ZipFilePath.IsPrincipal = true;
            ZipFilePath.IsRequired = true;
            ZipFilePath.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.Toggle))
            {
                FullEntryNames.AddWidget(ViewModelWidgetType.Toggle);
            }

#if WINDOWS
            if (IsWidgetSupported(ViewModelWidgetType.ActionButton))
            {
                ZipFilePath.AddMenuAction(new MenuAction()
                {
                    DisplayName = Resources.Common_ViewModel_BrowseForFile,
                    IsVisible = true,
                    IsMain = true,
                    Handler = _ => Task.Run(() =>
                    {
                        var ofd = new Microsoft.Win32.OpenFileDialog
                        {
                            Filter = "All files (*.*)|*.*",
                            Multiselect = false,
                            CheckFileExists = false
                        };

                        if (ofd.ShowDialog() == true)
                        {
                            ZipFilePath.Value = ofd.FileName;
                        }
                    })
                });
            }
#endif

        }
    }
}