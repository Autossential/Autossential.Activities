using System;
using System.Windows;

namespace Autossential.Activities.Design.PropertyEditors
{
    // Interaction logic for PropertyEditorResources.xaml
    public partial class PropertyEditorResources
    {
        private static ResourceDictionary resources;

        internal static ResourceDictionary GetResources()
        {
            if (resources == null)
            {
                var resourceLocator = new Uri($"{typeof(PropertyEditorResources).Assembly.GetName().Name};component/PropertyEditors/PropertyEditorResources.xaml", UriKind.RelativeOrAbsolute);
                resources = (ResourceDictionary)Application.LoadComponent(resourceLocator);
            }

            return resources;
        }

        internal static DataTemplate GetDataTemplate(string key)
        {
            return GetResources()[key] as DataTemplate;
        }
    }
}