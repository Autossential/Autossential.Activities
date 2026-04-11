using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class AddRangeToCollection<T> : CodeActivity
    {
        [RequiredArgument]
        public InArgument<ICollection<T>> Collection { get; set; }

        [RequiredArgument]
        public InArgument<IEnumerable<T>> Items { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var collection = Collection.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.AddRangeToCollection_Collection_DisplayName));
            var items = Items.Get(context) ?? throw new InvalidOperationException(ResourcesFn.Common_ErrorMsg_ValueNotSuppliedFormat(Resources.AddRangeToCollection_Collection_DisplayName));

            foreach (var item in items)
                collection.Add(item);
        }
    }
}
