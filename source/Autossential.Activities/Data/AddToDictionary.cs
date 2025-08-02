using Autossential.Activities.Properties;
using Autossential.Shared;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Autossential.Activities
{
    public class AddToDictionary : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument Dictionary { get; set; }

        [RequiredArgument]
        public InArgument Key { get; set; }

        [RequiredArgument]
        public InArgument Value { get; set; }

        public bool UpdateIfExists { get; set; } = true;
        public bool AutoInstantiate { get; set; } = true;

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

            if (Value != null)
            {
                if (keyValueTypes[1].IsAssignableFrom(Value.ArgumentType))
                {
                    metadata.AddRuntimeArgument(Value, Value.ArgumentType, nameof(Value), true);
                }
                else
                {
                    metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat(keyValueTypes[1].Name, nameof(Value)));
                }
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            var dict = Dictionary.Get(context);
            if (dict == null && AutoInstantiate)
            {
                try
                {
                    dict = Activator.CreateInstance(Dictionary.ArgumentType);
                }
                catch { }
            }

            if (dict == null)
            {
                throw new ArgumentNullException(ResourcesFn.Validation_InstanceIsNullFormat("Dictionary"));
            }

            var key = Key.Get(context) ?? throw new ArgumentNullException(nameof(Key));
            var value = Value.Get(context) ?? throw new ArgumentException(nameof(Value));

            var dictType = dict.GetType();
            var containsKeyMethod = dictType.GetMethod("ContainsKey");
            var addMethod = dictType.GetMethod("Add");
            var indexer = dictType.GetProperty("Item");

            if ((bool)containsKeyMethod.Invoke(dict, [key]))
            {
                if (UpdateIfExists)
                {
                    indexer.SetValue(dict, value, [key]);
                    Dictionary.Set(context, dict);
                    return;
                }

                throw new ArgumentException(ResourcesFn.AddToDictionary_ErrorMsg_KeyAlreadyExistsFormat(key));
            }

            addMethod.Invoke(dict, [key, value]);
            Dictionary.Set(context, dict);
        }
    }
}