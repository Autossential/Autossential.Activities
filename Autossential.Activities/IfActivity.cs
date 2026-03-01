using Autossential.Activities.Helpers;
using Autossential.Activities.Properties;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Autossential.Activities
{
    public sealed class IfActivity : NativeActivity
    {
        private Collection<Variable> _variables;

        [Browsable(false)]
        public Collection<Variable> Variables
        {
            get
            {
                return _variables ??= [];
            }
        }

        public bool CheckTrue { get; set; } = true;

        [Browsable(false)]
        public ActivityFunc<bool> Condition { get; set; }

        [Browsable(false)]
        public ActivityAction Body { get; set; }

        [Browsable(false)]
        public ActivityAction ElseBody { get; set; }

        public IfActivity()
        {
            Condition = new ActivityFunc<bool>();
            Constraints.Add(ActivityConstraints.ConditionalValidationConstraint<IfActivity>(p => p.Condition != null && p.Condition.Handler is Activity<bool>, Resources.IfActivity_ErrorMsg_ConditionReturnsBoolean));

            Body = new ActivityAction
            {
                Handler = new Sequence
                {
                    DisplayName = "Then"
                }
            };

            ElseBody = new ActivityAction
            {
                Handler = new Sequence
                {
                    DisplayName = "Else"
                }
            };
        }

        protected override void Execute(NativeActivityContext context)
        {
            context.ScheduleFunc(Condition, OnEvaluateConditionCompleted);
        }

        private void OnEvaluateConditionCompleted(NativeActivityContext context, ActivityInstance completedInstance, bool result)
        {
            if (result == CheckTrue)
            {
                if (Body != null)
                    context.ScheduleAction(Body);

                return;
            }

            if (ElseBody != null)
                context.ScheduleAction(ElseBody);
        }
    }
}