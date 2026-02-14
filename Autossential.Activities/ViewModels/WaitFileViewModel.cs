using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class WaitFileViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<bool> ContinueOnError { get; set; }
        public DesignInArgument<string> FilePath { get; set; }
        public DesignInArgument<string> DirectoryPath { get; set; }
        public DesignInArgument<string> SearchPattern { get; set; }
        public DesignInArgument<double> TimeoutSeconds { get; set; }
        public DesignInArgument<bool> WaitForExist { get; set; }
        public DesignInArgument<double> PollingIntervalSeconds { get; set; }
        public DesignProperty<bool> DynamicFile { get; set; }
        public DesignOutArgument<FileInfo> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            PersistValuesChangedDuringInit();

            if (IsWidgetSupported(ViewModelWidgetType.NullableBoolean))
            {
                ContinueOnError.AddWidget(ViewModelWidgetType.NullableBoolean);
                WaitForExist.AddWidget(ViewModelWidgetType.NullableBoolean);
            }

            var orderIndex = 0;

            DynamicFile.IsPrincipal = true;
            DynamicFile.OrderIndex = orderIndex++;

            FilePath.IsPrincipal = true;
            FilePath.OrderIndex = orderIndex++;

            DirectoryPath.IsPrincipal = true;
            DirectoryPath.OrderIndex = orderIndex++;

            SearchPattern.IsPrincipal = true;
            SearchPattern.Placeholder = "*.*";
            SearchPattern.OrderIndex = orderIndex++;

#if WINDOWS
            if (IsWidgetSupported(ViewModelWidgetType.ActionButton))
            {
                FilePath.AddMenuAction(new MenuAction()
                {
                    DisplayName = Resources.WaitFile_ViewModel_BrowseForFile,
                    IsVisible = true,
                    IsMain = true,
                    Handler = _ => Task.Run(() =>
                    {
                        var ofd = new Microsoft.Win32.OpenFileDialog
                        {
                            Filter = "All files (*.*)|*.*",
                            Multiselect = false
                        };

                        if (ofd.ShowDialog() == true)
                        {
                            FilePath.Value = ofd.FileName;
                        }
                    })
                });

                DirectoryPath.AddMenuAction(new MenuAction()
                {
                    DisplayName = Resources.WaitFile_ViewModel_BrowseForFolder,
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
                                        DirectoryPath.Value = dialog.SelectedPath;
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
            }
#endif
        }

        protected override void InitializeRules()
        {
            base.InitializeRules();

            Rule(nameof(DynamicFile), DynamicFileChanged);
        }

        private void DynamicFileChanged()
        {
            bool isDynamic = DynamicFile.Value; 

            DirectoryPath.IsVisible = DynamicFile.Value;
            SearchPattern.IsVisible = DynamicFile.Value;
            DirectoryPath.IsRequired = DynamicFile.Value;

            FilePath.IsVisible = !DynamicFile.Value;
            FilePath.IsRequired = !DynamicFile.Value;
        }
    }
}
