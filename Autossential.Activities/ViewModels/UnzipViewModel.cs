using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
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
                ZipFilePath.AddFileDialogMenuAction(false, "All files (*.*)|*.*");
                ExtractTo.AddFolderDialogMenuAction();
            }
#endif

        }
    }
}
