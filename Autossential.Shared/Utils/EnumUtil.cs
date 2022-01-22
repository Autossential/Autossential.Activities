using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Autossential.Shared.Utils
{
    public static class EnumUtil
    {
        public static Dictionary<string, Enum> EnumAsDictionary<TEnum>()
        {
            var list = new Dictionary<string, Enum>();
            var type = typeof(TEnum);
            foreach (Enum v in Enum.GetValues(type))
            {
                var name = type.GetEnumName(v);
                var field = type.GetField(name);
                var attr = field?.GetCustomAttribute<DescriptionAttribute>();
                list.Add(attr?.Description ?? name, v);
            }
            return list;
        }
    }
}