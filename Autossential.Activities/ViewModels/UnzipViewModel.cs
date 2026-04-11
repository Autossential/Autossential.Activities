using Autossential.Activities.Base;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class UnzipViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> ZipFilePath { get; set; }
        public DesignInArgument<string> ExtractTo { get; set; }
        public DesignInArgument<bool> Overwrite { get; set; }


        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;

            ZipFilePath.IsPrincipal = true;
            ZipFilePath.IsRequired = true;
            ZipFilePath.OrderIndex = orderIndex++;

            ExtractTo.IsPrincipal = true;
            ExtractTo.IsRequired = true;
            ExtractTo.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.Toggle))
            {
                Overwrite.Widget = new DefaultWidget
                {
                    Type = ViewModelWidgetType.Toggle
                };
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

            ExtractTo.AddMenuAction(new MenuAction()
            {
                DisplayName = Resources.Common_ViewModel_BrowseForFolder,
                IsVisible = true,
                IsMain = true,
                Handler = _ =>
                {
                    // cria uma TaskCompletionSource para controlar o resultado
                    var tcs = new TaskCompletionSource<bool>();
                    var thread = new Thread(() =>
                    {
                        try
                        {
                            using (var dialog = new FolderBrowserDialog())
                            {
                                dialog.ShowNewFolderButton = true;
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    ExtractTo.Value = dialog.SelectedPath;
                                }
                            }
                            tcs.SetResult(true);
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex);
                        }
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    return tcs.Task;
                }
            });
#endif
        }
    }
}
