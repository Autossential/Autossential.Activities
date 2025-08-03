using System.Activities.DesignViewModels;
using UiPath.Studio.Activities.Api;

namespace Autossential.Activities.ViewModels
{
    internal abstract class BaseViewModel : DesignPropertiesViewModel
    {
        private readonly IWorkflowDesignApi _workflowDesignerAPI;

        public BaseViewModel(IDesignServices services) : base(services)
        {
            _workflowDesignerAPI = services.GetService<IWorkflowDesignApi>();
        }

        public bool IsWidgetSupported(string widgetType)
        {
            if (!_workflowDesignerAPI.HasFeature(DesignFeatureKeys.WidgetSupportInfoService))
                return false;

            if (_workflowDesignerAPI.WidgetSupportInfoService?.IsWidgetSupported(widgetType) == false)
                return false;

            return true;
        }
    }

}
