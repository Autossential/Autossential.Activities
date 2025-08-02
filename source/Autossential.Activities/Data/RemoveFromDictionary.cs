using Autossential.Activities.Properties;
using Autossential.Shared;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Autossential.Activities
{
    public class RemoveFromDictionary : CodeActivity
    {
        [RequiredArgument]
        [DisplayName("Dictionary")]
        public InArgument Dictionary { get; set; }

        [RequiredArgument]
        public InArgument Key { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Dictionary == null)
            {
                metadata.AddRuntimeArgument(Dictionary, typeof(Dictionary<string, object>), nameof(Dictionary), true);
                return;
            }

            Type[] keyValueTypes = [];
            var argType = Dictionary.ArgumentType;
            if (argType.IsGenericType && argType.GenericTypeArguments.Length == 2 && typeof(IDictionary<,>).MakeGenericType(argType.GenericTypeArguments).IsAssignableFrom(argType))
            {
                metadata.AddRuntimeArgument(Dictionary, argType, nameof(Dictionary), true);
                keyValueTypes = argType.GenericTypeArguments;
            }
            else
            {
                metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("Dictionary<TKey, TValue>", "Dictionary"));
            }

            if (keyValueTypes.Length != 2)
                return;

            if (Key != null)
            {
                if (keyValueTypes[0].IsAssignableFrom(Key.ArgumentType))
                {
                    metadata.AddRuntimeArgument(Key, Key.ArgumentType, nameof(Key), true);
                }
                else
                {
                    metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat(keyValueTypes[0].Name, nameof(Key)));
                }
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var dict = Dictionary.Get(context) ?? throw new ArgumentNullException(nameof(Dictionary));
            var key = Key.Get(context) ?? throw new ArgumentNullException(nameof(Key));

            var dictType = dict.GetType();
            var containsKeyMethod = dictType.GetMethod("ContainsKey");
            var removeMethod = dictType.GetMethod("Remove", [key.GetType()]);

            if ((bool)containsKeyMethod.Invoke(dict, [key]))
            {
                removeMethod.Invoke(dict, [key]);
                Dictionary.Set(context, dict);
            }
        }
    }
}
