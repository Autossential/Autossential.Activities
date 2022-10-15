using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.ComponentModel;

namespace Autossential.Activities.Workflow
{
    public sealed class RepeatUntilFailure : Activity
    {
        [Browsable(false)]
        public ActivityDelegate Body { get; set; }
        public OutArgument<Exception> OutputException { get; set; }
        public OutArgument<int> Iterations { get; set; }
        public InArgument<int> MaximumRepetitions { get; set; }
        public InArgument<TimeSpan> LoopInterval { get; set; }


        public RepeatUntilFailure()
        {
            InitializeBody();
            Implementation = Build;
        }

        private void InitializeBody()
        {
            Body = new ActivityAction
            {
                Handler = new Sequence
                {
                    DisplayName = "Do"
                }
            };
        }

        private Activity Build()
        {
            var maxRep = new Variable<int>("maxRep", context => MaximumRepetitions.Expression == null ? 300 : MaximumRepetitions.Get(context));
            var counter = new Variable<int>("counter", context => maxRep.Get(context));
            var interval = new Variable<TimeSpan>("interval", context => LoopInterval.Expression == null ? TimeSpan.FromMilliseconds(100) : LoopInterval.Get(context));
            var ex = new DelegateInArgument<Exception>("ex");

            return new Sequence
            {
                Variables =
                {
                    maxRep ,
                    counter,
                    interval
                },

                Activities =
                {
                    new While
                    {
                        Condition = new GreaterThan<int, int, bool>
                        {
                            Left = counter,
                            Right = 0
                        },
                        Body = new Sequence
                        {
                            Activities =
                            {
                                new Assign<int>
                                {
                                    To = counter,
                                    Value = new Subtract<int, int, int>
                                    {
                                        Left = counter,
                                        Right = 1
                                    }
                                },
                                new Assign<int>
                                {
                                    To = new OutArgument<int>(context => Iterations.Get(context)),
                                    Value = new Subtract<int, int, int>
                                    {
                                        Left = maxRep,
                                        Right = counter
                                    }
                                },
                                new TryCatch
                                {
                                    Try = new Sequence
                                    {
                                        Activities =
                                        {
                                            new InvokeDelegate
                                            {
                                                Delegate = Body
                                            },
                                            new Delay
                                            {
                                                Duration = new InArgument<TimeSpan>(interval)
                                            }
                                        }
                                    },
                                    Catches =
                                    {
                                        new Catch<Exception>
                                        {
                                            Action = new ActivityAction<Exception>
                                            {
                                                Argument = ex,
                                                Handler = new Sequence
                                                {
                                                    Activities =
                                                    {
                                                        new Assign<Exception>
                                                        {
                                                            To = new OutArgument<Exception>(context => OutputException.Get(context)),
                                                            Value = new InArgument<Exception>(context => ex.Get(context))
                                                        },
                                                        new Assign<int>
                                                        {
                                                            To = counter,
                                                            Value = new InArgument<int>(_ => -1)
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
