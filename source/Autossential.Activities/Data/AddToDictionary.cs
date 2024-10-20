using Autossential.Activities.Properties;
using System;
using System.Activities;
using System.Collections.Generic;

namespace Autossential.Activities
{
    public class AddToDictionary<TKey, TValue> : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument<Dictionary<TKey, TValue>> ReferenceDictionary { get; set; }

        [RequiredArgument]
        public InArgument<TKey> Key { get; set; }

        [RequiredArgument]
        public InArgument<TValue> Value { get; set; }

        public InArgument<bool> UpdateIfExists { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var dict = ReferenceDictionary.Get(context) ?? new Dictionary<TKey, TValue>();
            var key = Key.Get(context);
            var value = Value.Get(context);

            if (dict.ContainsKey(key))
            {
                if (UpdateIfExists.Get(context))
                {
                    dict[key] = value;
                    ReferenceDictionary.Set(context, dict);
                    return;
                }
                throw new ArgumentException(Resources.AddToDictionary_ErrorMsg_KeyAlreadyExistsFormat(key));
            }
            dict.Add(key, value);
            ReferenceDictionary.Set(context, dict);
        }
    }
}
