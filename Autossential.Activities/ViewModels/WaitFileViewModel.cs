using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class WaitFileViewModel : BaseViewModel
    {
        public WaitFileViewModel(IDesignServices services) : base(services)
        {

        }

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

            int orderIndex = 0;

            ContinueOnError.IsPrincipal = false;
            ContinueOnError.IsRequired = false;
            ContinueOnError.Category = Resources.Categories_Common;
            ContinueOnError.DisplayName = Resources.Common_ContinueOnError_DisplayName;
            ContinueOnError.Placeholder = Resources.Common_ContinueOnError_Description;
            ContinueOnError.Tooltip = Resources.Common_ContinueOnError_Description;
            ContinueOnError.OrderIndex = orderIndex++;

            DynamicFile.IsPrincipal = true;
            DynamicFile.IsRequired = false;
            DynamicFile.Category = Resources.Categories_Options;
            DynamicFile.DisplayName = Resources.WaitFile_DynamicFile_DisplayName;
            DynamicFile.Placeholder = Resources.WaitFile_DynamicFile_Description;
            DynamicFile.Tooltip = Resources.WaitFile_DynamicFile_Description;
            DynamicFile.OrderIndex = orderIndex++;

            FilePath.IsPrincipal = true;
            FilePath.IsRequired = true;
            FilePath.Category = Resources.Categories_Input;
            FilePath.DisplayName = Resources.WaitFile_FilePath_DisplayName;
            FilePath.Placeholder = Resources.WaitFile_FilePath_Description;
            FilePath.Tooltip = Resources.WaitFile_FilePath_Description;
            FilePath.OrderIndex = orderIndex++;

            DirectoryPath.IsPrincipal = true;
            DirectoryPath.IsRequired = true;
            DirectoryPath.IsVisible = false;
            DirectoryPath.Category = Resources.Categories_Input;
            DirectoryPath.DisplayName = Resources.WaitFile_DirectoryPath_DisplayName;
            DirectoryPath.Placeholder = Resources.WaitFile_DirectoryPath_Description;
            DirectoryPath.Tooltip = Resources.WaitFile_DirectoryPath_Description;
            DirectoryPath.OrderIndex = orderIndex++;

            SearchPattern.IsPrincipal = true;
            SearchPattern.IsRequired = false;
            SearchPattern.IsVisible = false;
            SearchPattern.Category = Resources.Categories_Input;
            SearchPattern.DisplayName = Resources.WaitFile_SearchPattern_DisplayName;
            SearchPattern.Placeholder = "*.*";
            SearchPattern.Tooltip = Resources.WaitFile_SearchPattern_Description;
            SearchPattern.OrderIndex = orderIndex++;

            TimeoutSeconds.IsPrincipal = false;
            TimeoutSeconds.IsRequired = false;
            TimeoutSeconds.Category = Resources.Categories_Input;
            TimeoutSeconds.DisplayName = Resources.Common_TimeoutSeconds_DisplayName;
            TimeoutSeconds.Placeholder = Resources.Common_TimeoutSeconds_Description;
            TimeoutSeconds.Tooltip = Resources.Common_TimeoutSeconds_Description;
            TimeoutSeconds.OrderIndex = orderIndex++;

            WaitForExist.IsPrincipal = false;
            WaitForExist.IsRequired = false;
            WaitForExist.Category = Resources.Categories_Options;
            WaitForExist.DisplayName = Resources.WaitFile_WaitForExist_DisplayName;
            WaitForExist.Placeholder = Resources.WaitFile_WaitForExist_Description;
            WaitForExist.Tooltip = Resources.WaitFile_WaitForExist_Description;
            WaitForExist.OrderIndex = orderIndex++;

            PollingIntervalSeconds.IsPrincipal = false;
            PollingIntervalSeconds.IsRequired = false;
            PollingIntervalSeconds.Category = Resources.Categories_Options;
            PollingIntervalSeconds.DisplayName = Resources.Common_IntervalSeconds_DisplayName;
            PollingIntervalSeconds.Placeholder = Resources.Common_IntervalSeconds_Description;
            PollingIntervalSeconds.Tooltip = Resources.Common_IntervalSeconds_Description;
            PollingIntervalSeconds.OrderIndex = orderIndex++;

            Result.IsPrincipal = false;
            Result.IsRequired = false;
            Result.Category = Resources.Categories_Output;
            Result.DisplayName = Resources.WaitFile_Result_DisplayName;
            Result.Placeholder = Resources.WaitFile_Result_Description;
            Result.Tooltip = Resources.WaitFile_Result_Description;
            Result.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.NullableBoolean))
            {
                ContinueOnError.AddWidget(ViewModelWidgetType.NullableBoolean);
                WaitForExist.AddWidget(ViewModelWidgetType.NullableBoolean);
            }

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
            DirectoryPath.IsVisible = DynamicFile.Value;
            SearchPattern.IsVisible = DynamicFile.Value;
            DirectoryPath.IsRequired = DynamicFile.Value;

            FilePath.IsVisible = !DynamicFile.Value;
            FilePath.IsRequired = !DynamicFile.Value;
        }
    }
}
