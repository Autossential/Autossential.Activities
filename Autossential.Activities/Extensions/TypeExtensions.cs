namespace Autossential.Activities.Extensions
{
    internal static class TypeExtensions
    {
        extension(Type type)
        {
            public bool IsAssignableToGenericType(Type givenType)
            {
                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == type)
                    return true;

                foreach (var gt in givenType.GetInterfaces())
                {
                    if (gt.IsGenericType && gt.GetGenericTypeDefinition() == type)
                        return true;
                }

                return givenType.BaseType?.IsAssignableToGenericType(type) == true;
            }

            public Type GetInnerType()
            {
                if (type.IsGenericType)
                    return type.GenericTypeArguments[0];

                if (type.IsArray)
                    return type.GetElementType();

                return typeof(object);
            }
        }
    }
}