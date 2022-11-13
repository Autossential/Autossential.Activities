using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Linq;

namespace Autossential.Shared.Activities.Design
{
    public static class ModelItemExtensions
    {
        public static T? AsLiteral<T>(this ModelItem modelItem) where T : struct
        {
            return modelItem?.Properties["Expression"]?.Value?.GetCurrentValue() is Literal<T> currentValue
                ? new T?(currentValue.Value)
                : new T?();
        }

        public static ModelItem GetParentModelItem(this ModelItem ownerActivity, string propertyHierarchyPath)
        {
            if (string.IsNullOrWhiteSpace(propertyHierarchyPath))
                return ownerActivity;

            var parent = ownerActivity;
            var source = propertyHierarchyPath.Split(',');
            foreach (var name in source.Take(source.Length - 1))
                parent = parent?.Properties[name]?.Value;

            return parent;
        }
    }
}
