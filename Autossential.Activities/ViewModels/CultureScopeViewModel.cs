using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Globalization;

namespace Autossential.Activities.ViewModels
{
    internal class CultureScopeViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> Culture { get; set; }
        private readonly DataSource<string> _cultureDataSource = CreateCultureDataSource();

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            Culture.IsPrincipal = true;
            Culture.DataSource = _cultureDataSource;

            AddWidget(Culture, ViewModelWidgetType.AutoCompleteForExpression);
        }
    }
}