using Autossential.Activities.Properties;
using System;
using System.Activities;
using System.Collections.Generic;

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
            var data = Collection.Get(context);
            if (data == null)
                throw new ArgumentNullException(Resources.AddRangeToCollection_ErrorMsg_CollectionNull);

            var items = Items.Get(context);
            if (items == null)
                throw new ArgumentNullException(Resources.AddRangeToCollection_ErrorMsg_ItemsNull);

            foreach (var item in items)
                data.Add(item);
        }
    }
}