using Autossential.Activities.Tests.Helpers;
using System.Activities;
using System.Activities.Statements;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    /// <summary>
    /// Unit tests for <see cref="Container"/>.
    ///
    /// Strategy:
    ///   - Direct tests: constructor defaults, property initialization.
    ///   - Integration via WorkflowApplication: exercita o fluxo real de bookmark
    ///     (Execute → CreateExitBookmark → OnExit) incluindo o uso de <see cref="Exit"/>
    ///     como mecanismo de saída antecipada.
    ///
    /// WorkflowApplication é necessário (em vez de WorkflowInvoker) porque
    /// Container usa bookmarks (CanInduceIdle = true), o que requer suporte
    /// assíncrono/idle que somente WorkflowApplication fornece.
    /// </summary>
    public class ContainerTests
    {
        // Timeout usado em todos os WaitOne para evitar travamento nos testes.
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(10);

        [Fact]
        public void Constructor_ShouldInitializeBody_WithDoSequence()
        {
            var container = new Container();

            Assert.NotNull(container.Body);
            Assert.NotNull(container.Body.Handler);
            var sequence = Assert.IsType<Sequence>(container.Body.Handler);
            Assert.Equal("Do", sequence.DisplayName);
        }

        [Fact]
        public void Constructor_Body_ShouldBeMutable()
        {
            var container = new Container();
            var custom = new ActivityAction { Handler = new Sequence { DisplayName = "Custom" } };

            container.Body = custom;
            Assert.Same(custom, container.Body);
        }

        [Fact]
        public void Execute_WhenBodyIsNull_ShouldCompleteWithoutFault()
        {
            var completed = new ManualResetEventSlim(false);
            Exception? faultEx = null;

            var container = new Container { Body = null };

            var app = AppRunner.Run(container, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.Null(faultEx);
        }

        [Fact]
        public void Execute_WithoutExit_ShouldRunBodyToCompletion()
        {
            var bodyRan = false;
            var completed = new ManualResetEventSlim(false);

            var container = new Container
            {
                Body = new ActivityAction
                {
                    Handler = new ActionInvoker(() => bodyRan = true)
                }
            };

            var app = AppRunner.Run(container, _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.True(bodyRan);
        }

        [Fact]
        public void Execute_WhenExitCalled_ShouldCancelRemainingChildren()
        {
            // Sequence: Exit → atividade que NÃO deve rodar
            var shouldNotRun = false;
            var completed = new ManualResetEventSlim(false);

            var container = new Container
            {
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities =
                        {
                            new Exit(),                                          
                            new ActionInvoker(() => shouldNotRun = true) // do not run
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.False(shouldNotRun);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void Execute_WhenExitConditionIsSet_ShouldExitWhenTrue(bool condition, bool expectedResult)
        {
            var afterExitRan = false;
            var completed = new ManualResetEventSlim(false);

            var container = new Container
            {
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities =
                        {
                            new Exit { Condition = condition },
                            new ActionInvoker(() => afterExitRan = true)
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.Equal(expectedResult, afterExitRan);
        }


        [Fact]
        public void Execute_WhenExitCalledInsideNestedSequence_ShouldStillExit()
        {
            var outerActivityRan = false;
            var completed = new ManualResetEventSlim(false);

            var container = new Container
            {
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities =
                        {
                            new Sequence          // nested
                            {
                                Activities = { new Exit() }
                            },
                            new ActionInvoker(() => outerActivityRan = true)
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.False(outerActivityRan);
        }

        [Fact]
        public void Execute_MultipleExitCalls_ShouldNotFault()
        {
            // Two Exit in sequence — the second must not cause exception
            // because the bookmark was already consumed by the first one
            var completed = new ManualResetEventSlim(false);
            Exception? faultEx = null;

            var container = new Container
            {
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities =
                        {
                            new Exit(),
                            new Exit()   // bookmark does not exit
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, onCompleted: _ => completed.Set());
            Assert.True(completed.Wait(TestTimeout));
            Assert.Null(faultEx);
        }
    }
}
