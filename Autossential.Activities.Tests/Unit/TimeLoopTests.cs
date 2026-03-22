using Autossential.Activities.Tests.Helpers;
using System.Activities;
using System.Activities.Statements;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    public class TimeLoopTests
    {
        // ──────────────────────────────────────────────────────────────────────
        // 1. Constructor / Initialization
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Constructor_ShouldInitializeBody_WithDoSequence()
        {
            var loop = new TimeLoop();

            Assert.NotNull(loop.Body);
            Assert.NotNull(loop.Body.Handler);
            var sequence = Assert.IsType<Sequence>(loop.Body.Handler);
            Assert.Equal("Do", sequence.DisplayName);
        }

        [Fact]
        public void Constructor_Body_ShouldBeMutable()
        {
            var loop = new TimeLoop();
            var custom = new ActivityAction { Handler = new Sequence { DisplayName = "Custom" } };

            loop.Body = custom;

            Assert.Same(custom, loop.Body);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 2. Normal execution — iterations, interval seconds and timeout
        // ──────────────────────────────────────────────────────────────────────


        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, .5, 2)]
        [InlineData(1, .7, 2)]
        [InlineData(1, .25, 4)]
        public void Execute_ShouldStopIterating_AfterTimeoutExpires(int seconds, double interval, int expectedCount)
        {
            var count = 0;
            var loop = Build(TimeSpan.FromSeconds(seconds), interval, () => count++);
            WorkflowInvoker.Invoke(loop);
            Assert.Equal(expectedCount, count);
        }

        [Fact]
        public void Execute_WithZeroTimeout_ShouldNotRunBody()
        {
            var count = 0;
            var loop = Build(TimeSpan.Zero, 0, () => count++);
            Assert.Equal(0, count);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 3. IterationIndex
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_ShouldIncrementIterationIndex_EachIteration()
        {
            var count = -1; // iteration index is zero-based index, for correct comparison, counter starts in -1
            var outputs = WorkflowInvoker.Invoke(new OutputWrapper<int>
            {
                Factory = outputVar => new TimeLoop
                {
                    IterationIndex = new OutArgument<int>(outputVar),
                    Timeout = TimeSpan.FromSeconds(1),
                    IntervalSeconds = new InArgument<double>(.1),
                    Body = new ActivityAction
                    {
                        Handler = new ActionInvoker(() => count++)
                    }
                }
            });

            Assert.Equal(count, outputs["Output"]);
        }


        // ──────────────────────────────────────────────────────────────────────
        // 4. Result (bool)
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Result_ShouldBeTrue_WhenLoopEndsAfterTimeout()
        {
            var loop = Build(TimeSpan.FromMilliseconds(100), 0);
            Assert.True(WorkflowInvoker.Invoke(loop));
        }

        [Fact]
        public void Result_ShouldBeFalse_WhenLoopEndsViaExit()
        {
            var loop = new TimeLoop
            {
                Timeout = new InArgument<TimeSpan>(TimeSpan.FromMilliseconds(300)),
                Body = new ActivityAction
                {
                    Handler = new Sequence { Activities = { new Exit() } }
                }
            };

            Assert.False(WorkflowInvoker.Invoke(loop));
        }

        [Fact]
        public void Execute_WhenExitConditionIsFalse_ShouldContinueLoop()
        {
            var loop = new TimeLoop
            {
                Timeout = new InArgument<TimeSpan>(TimeSpan.FromMilliseconds(300)),
                Body = new ActivityAction
                {
                    Handler = new Sequence { Activities = { new Exit { Condition = false } } }
                }
            };

            Assert.True(WorkflowInvoker.Invoke(loop));
        }

        [Fact]
        public void Execute_WhenExitCalledInsideNestedSequence_ShouldStopLoop()
        {
            var outerRun = false;
            var loop = new TimeLoop
            {
                Timeout = new InArgument<TimeSpan>(TimeSpan.FromSeconds(1)),
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities =
                        {
                            new Sequence(),
                            new Sequence { Activities = { new Exit() }},
                            new ActionInvoker(() => outerRun = true)
                        }
                    }
                }
            };

            Assert.False(outerRun);
        }



        // ──────────────────────────────────────────────────────────────────────
        // 5. Canceling
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_WhenCanceled_ShouldMarkCanceled_AndStop()
        {
            var completed = new ManualResetEventSlim(false);
            ActivityInstanceState? completionState = null;

            var timeout = TimeSpan.FromSeconds(5);
            var loop = Build(timeout, .1);
            var app = AppRunner.Run(loop, args =>
            {
                completionState = args.CompletionState;
                completed.Set();
            });
            Thread.Sleep(300);
            app.Cancel();

            Assert.True(completed.Wait(1000));
            Assert.Equal(ActivityInstanceState.Canceled, completionState);
        }


        // ──────────────────────────────────────────────────────────────────────
        // 6. Fault propagation
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_WhenBodyThrows_ShouldFaultWorkflow()
        {
            var loop = Build(TimeSpan.FromSeconds(1), 0, () => throw new ApplicationException("Boom!"));

            Assert.Throws<ApplicationException>(() => WorkflowInvoker.Invoke(loop));
        }

        // ──────────────────────────────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────────────────────────────

        private static TimeLoop Build(TimeSpan timeout, double intervalSeconds, Action bodyAction = null!)
        {
            return new TimeLoop
            {
                Timeout = new InArgument<TimeSpan>(timeout),
                IntervalSeconds = new InArgument<double>(intervalSeconds),
                Body = new ActivityAction
                {
                    Handler = bodyAction == null ? new Sequence() : new ActionInvoker(bodyAction)
                }
            };
        }
    }
}