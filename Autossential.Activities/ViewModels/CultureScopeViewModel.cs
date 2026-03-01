using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;
using System.Globalization;

namespace Autossential.Activities.ViewModels
{
    internal class CultureScopeViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> CultureName { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            CultureName.IsPrincipal = true;
            CultureName.DataSource = DataSourceBuilder<string>
                .WithId(t => t)
                .WithLabel(t => $"{t} > {CultureInfo.GetCultureInfo(t).DisplayName}")
                .WithData([.. CultureInfo.GetCultures(CultureTypes.AllCultures).Select(c => c.Name)]).Build();

            if (IsWidgetSupported(ViewModelWidgetType.AutoCompleteForExpression))
                CultureName.AddWidget(ViewModelWidgetType.AutoCompleteForExpression);
        }
    }
}