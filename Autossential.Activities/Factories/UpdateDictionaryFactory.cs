using Autossential.Activities.Extensions;
using Designer.BackEnd;
using System.Activities;

namespace Autossential.Activities.Factories
{
    internal class UpdateDictionaryFactory(Func<string, Type, object> createArgument) : IActivityFactory
    {
        private readonly Func<string, Type, object> _createArgument = createArgument;

        public IReadOnlyCollection<RequiredProperty> GetRequiredProperties()
        {
            return [
                new RequiredProperty(new Property("Dictionary", null, PropertyType.Object, null){ IsExpression=true })
                {
                    IsTypeCompatible = type => typeof(Dictionary<,>).IsAssignableToGenericType(type)
                },
                new RequiredProperty(new Property("Entries", null, PropertyType.Object, null){ IsExpression=true })
                {
                    IsTypeCompatible = type => typeof(Dictionary<,>).IsAssignableToGenericType(type)
                }
            ];
        }

        public Activity Create(CreateActivityContext context)
        {
            var (_, type) = (Tuple<string, Type>)context.RequiredValues.First();
            if (type == typeof(object) || type.GenericTypeArguments.Length != 2)
                return new UpdateDictionary<string, object>();

            var activity = typeof(UpdateDictionary<,>).MakeGenericType(type.GenericTypeArguments);
            return (Activity)Activator.CreateInstance(activity);
        }
    }
}