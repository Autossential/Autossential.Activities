using Autossential.Activities.Properties;
using System;
using System.Activities;
using System.Collections.Generic;

namespace Autossential.Activities
{
    public class RemoveFromDictionary<TKey, TValue> : CodeActivity
    {
        [RequiredArgument]
        public InArgument<Dictionary<TKey, TValue>> InputDictionary { get; set; }

        [RequiredArgument]
        public InArgument<TKey> Key { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (InputDictionary.Get(context) is Dictionary<TKey, TValue> dict)
            {
                var key = Key.Get(context);
                if (dict.ContainsKey(key))
                    dict.Remove(key);
            }
            else
            {
                throw new ArgumentNullException(nameof(InputDictionary), Resources.RemoveFromDictionary_ErrorMsg_NullDictionary);
            };
        }
    }
}
