using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;


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
        public DesignInArgument<bool> ContinueOnError { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            Folder.IsPrincipal = true;
            Folder.IsRequired = true;
            SearchPattern.IsPrincipal = true;
            SearchPattern.Placeholder = "*.*";

            AddWidget(DeleteEmptyFolders, ViewModelWidgetType.Toggle);
            AddWidget(ContinueOnError, ViewModelWidgetType.NullableBoolean);

#if WINDOWS
            if (IsWidgetSupported(ViewModelWidgetType.ActionButton))
                Folder.AddFolderDialogMenuAction();
#endif
        }
    }
}