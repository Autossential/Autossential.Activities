using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Models;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Globalization;
using System.Text;

namespace Autossential.Activities.ViewModels
{
    internal class LoadDataFileViewModel : BaseViewModel
    {
        public DesignInArgument<string> FilePath { get; set; }
        public DesignInArgument<string> Encoding { get; set; }
        public DesignInArgument<CultureInfo> Culture { get; set; }
        public DesignOutArgument<DataNode> Result { get; set; }

        private readonly DataSource<string> _encodingDataSource;
        private readonly DataSource<string> _cultureDataSource;

        public LoadDataFileViewModel(IDesignServices services) : base(services)
        {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _encodingDataSource = CreateEncodingDataSource();
            _cultureDataSource = CreateCultureDataSource();
        }

        protected override void InitializeModel()
        {
            base.InitializeModel();

            FilePath.IsPrincipal = true;
#if WINDOWS
            if (IsWidgetSupported(ViewModelWidgetType.ActionButton))
                FilePath.AddFileDialogMenuAction(true, "Data files (*.json;*.yaml;*.yml;*.txt)|*.json;*.yaml;*.yml;*.txt|All files (*.*)|*.*");
#endif

            Encoding.DataSource = _encodingDataSource;
            Culture.DataSource = _cultureDataSource;

            if (IsWidgetSupported(ViewModelWidgetType.AutoCompleteForExpression))
            {
                Encoding.AddWidget(ViewModelWidgetType.AutoCompleteForExpression);
                Culture.AddWidget(ViewModelWidgetType.AutoCompleteForExpression);
            }
        }
    }
}
