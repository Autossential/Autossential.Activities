using System.Activities.DesignViewModels;
using System.Globalization;
using UiPath.Studio.Activities.Api;
using UiPath.Studio.Activities.Api.ProjectProperties;

namespace Autossential.Activities.Base
{
    public abstract class BaseViewModel(IDesignServices services) : DesignPropertiesViewModel(services)
    {
        private readonly IWorkflowDesignApi _workflowDesignerAPI = services.GetService<IWorkflowDesignApi>();

        public bool IsWidgetSupported(string widgetType)
        {
            if (!_workflowDesignerAPI.HasFeature(DesignFeatureKeys.WidgetSupportInfoService))
                return false;

            if (_workflowDesignerAPI.WidgetSupportInfoService?.IsWidgetSupported(widgetType) == false)
                return false;

            return true;
        }

        protected IWorkflowDesignApi GetWorkflowDesignApi() => _workflowDesignerAPI;

        public bool IsCSharpProject() => _workflowDesignerAPI.ProjectPropertiesService.GetExpressionLanguage() == (int)ExpressionLanguage.CSharp;

        protected static DataSource<string> CreateCultureDataSource() => DataSourceBuilder<string>
                .WithId(t => t)
                .WithLabel(t => $"{CultureInfo.GetCultureInfo(t).DisplayName} | {t}")
                .WithData([.. CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(p => p.Name).Select(c => c.Name)]).Build();

        protected static DataSource<string> CreateEncodingDataSource() => DataSourceBuilder<string>
                .WithId(s => s)
                .WithLabel(s => s)
                .WithData([.. System.Text.Encoding.GetEncodings().OrderBy(p => p.DisplayName).Select(p => p.DisplayName)]).Build();
    }
}
