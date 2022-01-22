using System.Activities;
using System.Collections.Generic;
using System.Linq;

namespace Autossential.Shared
{
    public static class ArgumentExtensions
    {
        public static bool IsArgumentTypeCompatible<T1>(this Argument arg)
        {
            var type = typeof(T1);
            if ((type.IsInterface || type.IsClass) && type.IsAssignableFrom(arg.ArgumentType))
                return true;

            return type == arg.ArgumentType;
        }
        public static bool IsArgumentTypeAnyCompatible<T1, T2>(this Argument arg)
        {
            return IsArgumentTypeCompatible<T1>(arg)
                || IsArgumentTypeCompatible<T2>(arg);
        }

        public static T[] GetAsArray<T>(this Argument arg, CodeActivityContext context)
        {
            var result = new HashSet<T>();
            var value = arg?.Get(context) ?? default(T);

            if (value == null)
                return result.ToArray();

            void forEachItem(IEnumerable<T> collection)
            {
                foreach (var v in collection)
                {
                    result.Add(v);
                }
            }

            if (value is IList<T> valueList)
            {
                forEachItem(valueList);
            }
            else if (value is T[] valueArray)
            {
                forEachItem(valueArray);
            }
            else
            {
                result.Add((T)value);
            }

            return result.ToArray();
        }
    }
}
