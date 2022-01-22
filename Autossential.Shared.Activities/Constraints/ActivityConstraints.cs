using System;
using System.Activities;
using System.Activities.Statements;
using System.Activities.Validation;

namespace Autossential.Shared.Activities.Constraints
{
    public static class ActivityConstraints
    {
        public static Constraint CreateConstraint<TActivity, TScopeActivity>(string validationMessage)
        {
            return CreateConstraint<TActivity>(p => p is TScopeActivity, validationMessage);
        }

        public static Constraint CreateConstraint<TActivity>(Func<Activity, bool> condition, string validationMessage)
        {
            var arg = new DelegateInArgument<TActivity>();
            var context = new DelegateInArgument<ValidationContext>();
            var result = new Variable<bool>();
            var parent = new DelegateInArgument<Activity>();
            return new Constraint<TActivity>
            {
                Body = new ActivityAction<TActivity, ValidationContext>
                {
                    Argument1 = arg,
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
