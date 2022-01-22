using Autossential.Core.Enums;

using System.Activities;

namespace Autossential.Activities
{
    public sealed class Stopwatch : CodeActivity
    {
        [RequiredArgument]
        public InOutArgument<System.Diagnostics.Stopwatch> ReferenceStopwatch { get; set; }

        public StopwatchMethods Method { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var obj = ReferenceStopwatch.Get(context) ?? new System.Diagnostics.Stopwatch();

            switch (Method)
            {
                case StopwatchMethods.Start:
                    obj.Start();
                    break;

                case StopwatchMethods.Stop:
                    obj.Stop();
                    break;

                case StopwatchMethods.Reset:
                    obj.Reset();
                    break;

                case StopwatchMethods.Restart:
                    obj.Restart();
                    break;
            }

            ReferenceStopwatch.Set(context, obj);
        }
    }
}