using Autossential.Activities.Properties;
using System.Activities;
using System.Activities.Statements;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Autossential.Activities
{
    public sealed class WhenDo : NativeActivity
    {
        private Collection<Variable> _variables;

        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get
            {
                return _variables ?? (_variables = new Collection<Variable>());
            }
        }

        public WhenDo()
        {
            Body = new ActivityAction
            {
                Handler = new Sequence
                {
                    DisplayName = "Action"
                }
            };

            ElseBody = new ActivityAction
            {
                Handler = new Sequence
                {
                    DisplayName = "Action Else"
                }
            };
        }

        public bool Inverted { get; set; }

        public bool WithElse { get; set; }

        [Browsable(false)]
        public Activity<bool> Condition { get; set; }

        [Browsable(false)]
        public ActivityAction Body { get; set; }

        [Browsable(false)]
        public ActivityAction ElseBody { get; set; }

        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            if (Condition == null)
                metadata.AddValidationError(ResourcesFn.Validation_ValueErrorFormat(nameof(Condition)));
        }

        protected override void Execute(NativeActivityContext context)
        {
            context.ScheduleActivity(Condition, new CompletionCallback<bool>(OnEvaluateConditionCompleted));
        }

        private void OnEvaluateConditionCompleted(NativeActivityContext context, ActivityInstance completedInstance, bool result)
        {
            if (result == !Inverted)
            {
                if (Body != null)
                    context.ScheduleAction(Body);

                return;
            }

            if (WithElse && ElseBody != null)
                context.ScheduleAction(ElseBody);
        }
    }
}