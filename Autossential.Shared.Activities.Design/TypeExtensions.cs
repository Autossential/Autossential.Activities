using System;
using System.Collections.Generic;
using System.Linq;

namespace Autossential.Shared.Activities.Design
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type type)
        {
            if (type.IsGenericType)
            {
                return type.Assembly.GetTypes()
                    .Where(p =>
                        p.BaseType.IsGenericType
                        && p.BaseType.GetGenericTypeDefinition() == type);
            }

            return type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type));
        }
    }
}
