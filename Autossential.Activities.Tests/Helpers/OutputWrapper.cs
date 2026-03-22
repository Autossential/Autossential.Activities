using System.Activities;

namespace Autossential.Activities.Tests.Helpers
{
    internal sealed class OutputWrapper<T> : NativeActivity
    {
        private readonly Variable<T> _output = new();

        public OutArgument<T> Output { get; set; } = new();
        public Func<Variable<T>, Activity> Factory { get; set; }

        private Activity _inner;

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            _inner = Factory(_output);
            metadata.AddImplementationVariable(_output);
            metadata.AddImplementationChild(_inner);
            base.CacheMetadata(metadata);
        }

        protected override void Execute(NativeActivityContext context)
            => context.ScheduleActivity(_inner, OnCompleted);

        private void OnCompleted(NativeActivityContext context, ActivityInstance instance)
            => Output.Set(context, _output.Get(context));
    }
}
