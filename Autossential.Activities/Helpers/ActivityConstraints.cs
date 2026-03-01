using System.Activities;
using System.Activities.Statements;
using System.Activities.Validation;

namespace Autossential.Activities.Helpers
{
    public static class ActivityConstraints
    {
        public static Constraint ConditionalValidationConstraint<TActivity>(Func<TActivity, bool> condition, string validationMessage)
        {
            var element = new DelegateInArgument<TActivity>();
            var context = new DelegateInArgument<ValidationContext>();
            return new Constraint<TActivity>
            {
                Body = new ActivityAction<TActivity, ValidationContext>
                {
                    Argument1 = element,
                    Argument2 = context,
                    Handler = new AssertValidation
                    {
                        Assertion = new InArgument<bool>(ctx => condition(element.Get(ctx))),
                        Message = new InArgument<string>(validationMessage)
                    }
                }
            };
        }

        public static Constraint ParentValidationConstraint<TActivity>(Func<Activity, bool> condition, string validationMessage)
        {
            var element = new DelegateInArgument<TActivity>();
            var context = new DelegateInArgument<ValidationContext>();
            var result = new Variable<bool>();
            var parent = new DelegateInArgument<Activity>();
            return new Constraint<TActivity>
            {
                Body = new ActivityAction<TActivity, ValidationContext>
                {
                    Argument1 = element,
                    Argument2 = context,
                    Handler = new Sequence()
                    {
                        Variables = { result },
                        Activities =
                        {
                            new ForEach<Activity>
                            {
                                Values = new GetParentChain{ ValidationContext = context },
                                Body = new ActivityAction<Activity>
                                {
                                    Argument = parent,
                                    Handler = new If
                                    {
                                        Condition = new InArgument<bool>(ctx => condition(parent.Get(ctx))),
                                        Then = new Assign<bool>
                                        {
                                            Value = true,
                                            To = result
                                        }
                                    }
                                }
                            },
                            new AssertValidation
                            {
                                Assertion = new InArgument<bool>(result),
                                Message = new InArgument<string>(validationMessage)
                            }
                        }
                    }
                }
            };
        }
    }
}