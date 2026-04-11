using System.Activities;

namespace Autossential.Activities.Tests.Helpers
{
    internal sealed class CaptureVariable<T>(Variable<T> variable, Action<T> capture) : CodeActivity
    {
        private readonly Variable<T> variable = variable;
        private readonly Action<T> capture = capture;

        protected override void Execute(CodeActivityContext context)
        {
            capture(variable.Get(context));
        }
    }
}