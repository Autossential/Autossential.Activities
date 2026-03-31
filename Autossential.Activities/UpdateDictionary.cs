using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class UpdateDictionary<TKey, TValue> : CodeActivity
    {
        [RequiredArgument]
        public InArgument<Dictionary<TKey, TValue>> Dictionary { get; set; }

        [RequiredArgument]
        public InArgument<Dictionary<TKey, TValue>> Entries { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var dict = Dictionary.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.UpdateDictionary_DisplayName));
            var entries = Entries.Get(context);
            if (entries == null)
                return;

            foreach (var (k, v) in entries)
                dict[k] = v;
        }
    }
}
