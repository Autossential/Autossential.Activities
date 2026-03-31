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


//public static bool IsAssignableToGenericType(this Type genericType, Type givenType)
//{
//    foreach (Type type in givenType.GetInterfaces())
//    {
//        if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
//            return true;
//    }
//    if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
//        return true;
//    Type baseType = givenType.BaseType;
//    return !(baseType == (Type)null) && baseType.IsAssignableToGenericType(genericType);
//}

//public static Type GetInnerType(this Type baseType)
//{
//    Type innerType = typeof(object);
//    if (baseType.IsGenericType)
//        innerType = ((IEnumerable<Type>)baseType.GenericTypeArguments).First<Type>();
//    else if (baseType.IsArray)
//        innerType = baseType.GetElementType();
//    return innerType;
//}