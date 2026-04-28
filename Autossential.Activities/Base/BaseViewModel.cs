using Autossential.Activities.Extensions;
using System.Activities.DesignViewModels;
using System.Globalization;
using UiPath.Studio.Activities.Api;
using UiPath.Studio.Activities.Api.ProjectProperties;
using UiPath.Studio.Activities.Api.Widgets;

namespace Autossential.Activities.Base
{
    public abstract class BaseViewModel : DesignPropertiesViewModel
    {
        private readonly IWorkflowDesignApi _workflowDesignerAPI;
        private readonly IWidgetSupportInfoService _widgetSupportInfoService;

        protected BaseViewModel(IDesignServices services) : base(services)
        {
            _workflowDesignerAPI = services.GetService<IWorkflowDesignApi>();
            if (!_workflowDesignerAPI.HasFeature(DesignFeatureKeys.WidgetSupportInfoService))
                return;

            _widgetSupportInfoService = _workflowDesignerAPI.WidgetSupportInfoService;
        }

        public bool IsWidgetSupported(string widgetType)
        {
            return _widgetSupportInfoService is not null && _widgetSupportInfoService.IsWidgetSupported(widgetType);
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

        /// <summary>
        /// Adds a widget of the specified type to the given design property if the widget type is supported.
        /// </summary>
        /// <typeparam name="T">The type of the value held by the design property.</typeparam>
        /// <param name="property">The design property to which the widget will be added.</param>
        /// <param name="widgetType">The type of widget to add. Must be a supported widget type.</param>
        protected void AddWidget<T>(DesignProperty<T> property, string widgetType)
        {
            if (IsWidgetSupported(widgetType))
                property.AddWidget(widgetType);
        }
    }
}