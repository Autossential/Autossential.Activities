using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
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
                FilePath.AddFileDialogMenuAction(true, "All files (*.*)|*.*");
                DirectoryPath.AddFolderDialogMenuAction();
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
