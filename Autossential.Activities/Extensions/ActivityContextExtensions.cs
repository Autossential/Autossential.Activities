using System.Activities;
using UiPath.Robot.Activities.Api;

namespace Autossential.Activities.Extensions
{
    public static class ActivityContextExtensions
    {
        public static IExecutorRuntime GetExecutorRuntime(this ActivityContext context) => context.GetExtension<IExecutorRuntime>();
    }
}
