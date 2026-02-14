using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using Autossential.Activities.Properties;
using Autossential.Activities.Extensions;
using System.Activities;


namespace Autossential.Activities.ViewModels
{
    internal class CleanUpFolderViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> Folder { get; set; }
        public DesignInArgument<string> SearchPattern { get; set; }
        public DesignInArgument<DateTime?> LastWriteTime { get; set; }
        public DesignInArgument<bool> DeleteEmptyFolders { get; set; }
        public DesignProperty<SearchOption> SearchOption { get; set; }
        public DesignOutArgument<int> FilesDeleted { get; set; }
        public DesignOutArgument<int> FoldersDeleted { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            Folder.IsPrincipal = true;
            Folder.IsRequired = true;
            SearchPattern.IsPrincipal = true;
            SearchPattern.Placeholder = "*.*";

            if (IsWidgetSupported(ViewModelWidgetType.Toggle))
            {
                DeleteEmptyFolders.AddWidget(ViewModelWidgetType.Toggle);
            }

#if WINDOWS
            Folder.AddMenuAction(new MenuAction()
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
                                    Folder.Value = dialog.SelectedPath;
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