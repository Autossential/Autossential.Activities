using Autossential.Activities.Extensions;
using Designer.BackEnd;
using System.Activities;

namespace Autossential.Activities.Factories
{
    public class AddRangeToCollectionFactory(Func<string, Type, object> createArgument) : IActivityFactory
    {
        private readonly Func<string, Type, object> _createArgument = createArgument;

        public IReadOnlyCollection<RequiredProperty> GetRequiredProperties()
        {
            return [
                new RequiredProperty(new Property("Collection", null, PropertyType.Object, null){ IsExpression=true })
                {
                    IsTypeCompatible = type => typeof(ICollection<>).IsAssignableToGenericType(type)
                },
                new RequiredProperty(new Property("ItemsToAdd", null, PropertyType.Object, null){ IsExpression=true })
                {
                    IsTypeCompatible = type => typeof(IEnumerable<>).IsAssignableToGenericType(type)
                },
            ];
        }

        public Activity Create(CreateActivityContext context)
        {
            var (_, type) = (Tuple<string, Type>)context.RequiredValues.First();
            var activity = typeof(AddRangeToCollection<>).MakeGenericType(type.GetInnerType());
            return (Activity)Activator.CreateInstance(activity);
        }
    }
}