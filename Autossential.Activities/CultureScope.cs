using System.Activities;
using System.Activities.Statements;
using System.ComponentModel;
using System.Globalization;

namespace Autossential.Activities
{
    public sealed class CultureScope : NativeActivity
    {
        [RequiredArgument]
        public InArgument<string> Culture { get; set; }

        [Browsable(false)]
        public ActivityAction Body { get; set; }

        protected override bool CanInduceIdle => true;

        public CultureScope()
        {
            Body = new ActivityAction
            {
                Handler = new Sequence { DisplayName = "Do" }
            };
        }

        protected override void Execute(NativeActivityContext context)
        {
            var culture = Culture.Get(context) ?? "";
            context.Properties.Add(typeof(CultureScopeHandler).FullName, new CultureScopeHandler(CultureInfo.GetCultureInfo(culture)));
            context.ScheduleAction(Body);
        }

        private class CultureScopeHandler(CultureInfo cultureInfo) : Handle, IExecutionProperty
        {
            private readonly CultureInfo _cultureInfo = cultureInfo;
            private CultureInfo _originalCulture;

            public void CleanupWorkflowThread()
            {
                Thread.CurrentThread.CurrentCulture = _originalCulture;
            }

            public void SetupWorkflowThread()
            {
                _originalCulture = CultureInfo.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = _cultureInfo;
            }
        }
    }
}
