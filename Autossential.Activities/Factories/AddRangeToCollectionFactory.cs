using Autossential.Activities.Extensions;
using Designer.BackEnd;
using System.Activities;
using System.Reflection;

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
            var obj = context.RequiredValues.First();
            if (obj is Tuple<string, Type> tuple)
            {
                var (_, type) = tuple;
                var result = typeof(AddRangeToCollectionFactory).GetMethod(nameof(CreateActivity), BindingFlags.Static | BindingFlags.NonPublic)
                    .MakeGenericMethod(type.GetInnerType()).Invoke(this, []);
                return (Activity)result;
            }
            return null;
        }

        private static Activity CreateActivity<T>() => new AddRangeToCollection<T>();
    }
}