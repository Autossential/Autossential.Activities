using Autossential.Activities.Models;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class MergeData : CodeActivity
    {
        [RequiredArgument]
        public InArgument<DataNode> Source { get; set; }

        [RequiredArgument]
        public InOutArgument<DataNode> Destination { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            var source = Source.Get(context);
            var destination = Destination.Get(context);
            destination.Merge(source);
            Destination.Set(context, destination);
        }
    }
}
