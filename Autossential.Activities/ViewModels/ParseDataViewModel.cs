using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using Autossential.Activities.Models;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Globalization;

namespace Autossential.Activities.ViewModels
{
    internal class ParseDataViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> Content { get; set; }
        public DesignInArgument<CultureInfo> Culture { get; set; }
        public DesignOutArgument<DataNode> Result { get; set; }

        private readonly DataSource<string> _cultureDataSource = CreateCultureDataSource();

        protected override void InitializeModel()
        {
            base.InitializeModel();
            Content.IsPrincipal = true;

            Culture.DataSource = _cultureDataSource;
            if (IsWidgetSupported(ViewModelWidgetType.AutoCompleteForExpression))
                Culture.AddWidget(ViewModelWidgetType.AutoCompleteForExpression);
        }
    }
}
