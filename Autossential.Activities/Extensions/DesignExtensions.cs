using System.Activities.DesignViewModels;

namespace Autossential.Activities.Extensions
{
    internal static class DesignExtensions
    {
        public static void AddWidget<T>(this DesignInArgument<T> argument, string widgetType)
        {
            argument.Widget = new DefaultWidget
            {
                Type = widgetType,
            };
        }
    }
}
