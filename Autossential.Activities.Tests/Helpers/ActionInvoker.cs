using System.Activities;

namespace Autossential.Activities.Tests.Helpers
{
    internal sealed class ActionInvoker(Action action) : CodeActivity
    {
        private readonly Action _action = action;

        protected override void Execute(CodeActivityContext context) => _action();
    }
}