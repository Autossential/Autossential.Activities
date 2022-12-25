using Autossential.Activities.Properties;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.ComponentModel;
using DiagStopwatch = System.Diagnostics.Stopwatch;

namespace Autossential.Activities.Workflow
{
    public sealed class TimeLoop : Activity
    {
        [Browsable(false)]
        public ActivityDelegate Body { get; set; }
        public InArgument<TimeSpan> Timer { get; set; }
        public InArgument<bool> ExitOnException { get; set; }
        public InArgument<TimeSpan> LoopInterval { get; set; }
        public OutArgument<Exception> OutputException { get; set; }
        public OutArgument<int> IterationNumber { get; set; }

        protected override void CacheMetadata(ActivityMetadata metadata)
        {
            if (Timer == null)
                metadata.AddValidationError(Resources.Validation_ValueErrorFormat(nameof(Timer)));

            base.CacheMetadata(metadata);
        }

        public TimeLoop()
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
            var interval = new Variable<TimeSpan>("interval", context => LoopInterval.Expression == null ? TimeSpan.Zero : LoopInterval.Get(context));
            var sw = new Variable<DiagStopwatch>("sw");
            var ex = new DelegateInArgument<Exception>("ex");

            return new Sequence
            {
                Variables = { interval, sw },
                Activities =
                {
                    new If
                    {
                        Condition = new LessThanOrEqual<TimeSpan, TimeSpan, bool>
                        {
                            Left = new InArgument<TimeSpan>(context => Timer.Get(context)),
                            Right = new InArgument<TimeSpan>(TimeSpan.Zero)
                        },
                        Then = new Throw
                        {
                            Exception = new InArgument<Exception>(_ => new ArgumentOutOfRangeException(nameof(Timer)))
                        }
                    },
                    new Assign<DiagStopwatch>
                    {
                        To = sw,
                        Value = new InArgument<DiagStopwatch>(_ => DiagStopwatch.StartNew())
                    },
                    new DoWhile
                    {
                        Condition = new And<bool, bool, bool>
                        {
                            Left = new LessThanOrEqual<TimeSpan, TimeSpan, bool>
                            {
                                Left = new InArgument<TimeSpan>(context => sw.Get(context).Elapsed),
                                Right = new InArgument<TimeSpan>(context => Timer.Get(context))
                            },
                            Right = new Not<bool, bool>
                            {
                                Operand=new And<bool, bool, bool>
                                {
                                    Left = new Equal<bool, bool, bool>
                                    {
                                        Left = new InArgument<bool>(context => ExitOnException.Get(context)),
                                        Right = new InArgument<bool>(true)
                                    },
                                    Right = new NotEqual<Exception, Exception, bool>
                                    {
                                        Left = new InArgument<Exception>(context => OutputException.Get(context)),
                                        Right = new InArgument<Exception>(_ => null)
                                    }
                                }
                            }
                        },
                        Body = new Sequence
                        {
                            Activities =
                            {
                                new TryCatch
                                {
                                    Try = new Sequence
                                    {
                                        Activities =
                                        {
                                            new Assign<int>
                                            {
                                                To = new OutArgument<int>(context => IterationNumber.Get(context)),
                                                Value = new InArgument<int>(context => IterationNumber.Get(context) + 1)
                                            },
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

                                                        new If
                                                        {
                                                            Condition = new Not<bool, bool>
                                                            {
                                                                Operand = new InArgument<bool>(context => ExitOnException.Get(context))
                                                            },
                                                            Then = new Delay
                                                            {
                                                                Duration = new InArgument<TimeSpan>(interval)
                                                            }
                                                        }
                                                   }
                                                }
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    }
                }
            };
        }
    }
}