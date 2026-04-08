using System.Activities.DesignViewModels;

namespace Autossential.Activities.Extensions
{
    internal static class DesignExtensions
    {
        extension<T>(DesignProperty<T> property)
        {
            /// <summary>
            /// Adds a widget of the specified type to the current property.
            /// </summary>
            /// <param name="widgetType">The type of widget to add. Cannot be null or empty.</param>
            public void AddWidget(string widgetType)
            {
                property.Widget = new DefaultWidget
                {
                    Type = widgetType,
                };
            }
        }
    }
}
