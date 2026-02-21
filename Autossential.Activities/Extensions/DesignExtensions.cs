using System.Activities.DesignViewModels;

namespace Autossential.Activities.Extensions
{
    internal static class DesignExtensions
    {
        extension<T>(DesignProperty<T> argument)
        {
            public void AddWidget(string widgetType)
            {
                argument.Widget = new DefaultWidget
                {
                    Type = widgetType,
                };
            }
        }
    }
}
