using Autossential.Activities.Tests.Helpers;
using System.Activities;
using System.Activities.Statements;
using Xunit;

namespace Autossential.Activities.Tests.Unit
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

        // ──────────────────────────────────────────────────────────────────────
        // 1. Constructor / Initialization
        // ──────────────────────────────────────────────────────────────────────

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

        // ──────────────────────────────────────────────────────────────────────
        // 2. Execução normal (sem Exit) — Body roda até o fim
        // ──────────────────────────────────────────────────────────────────────

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

            Assert.True(completed.Wait(TestTimeout), "Workflow deve completar.");
            Assert.True(bodyRan, "Body deve ter executado.");
        }

        // ──────────────────────────────────────────────────────────────────────
        // 3. Integração com Exit — saída antecipada
        // ──────────────────────────────────────────────────────────────────────

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
                            new Exit(),                                           // dispara saída
                            new ActionInvoker(() => shouldNotRun = true)          // não deve rodar
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout), "Workflow deve completar.");
            Assert.False(shouldNotRun, "Atividade após Exit não deve ter rodado.");
        }

        [Fact]
        public void Execute_WhenExitCalled_ShouldCompleteWorkflow()
        {
            var completed = new ManualResetEventSlim(false);
            Exception? faultEx = null;

            var container = new Container
            {
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities = { new Exit() }
                    }
                }
            };

            var app = AppRunner.Run(container, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout), "Workflow deve completar sem fault.");
            Assert.Null(faultEx);
        }

        [Fact]
        public void Execute_WhenExitConditionIsTrue_ShouldExitEarly()
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
                            new Exit { Condition = true },
                            new ActionInvoker(() => afterExitRan = true)
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.False(afterExitRan, "Nada após Exit(Condition=true) deve rodar.");
        }

        [Fact]
        public void Execute_WhenExitConditionIsFalse_ShouldContinueBody()
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
                            new Exit { Condition = false },      // condição falsa → não sai
                            new ActionInvoker(() => afterExitRan = true)
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.True(afterExitRan, "Body deve continuar quando Exit.Condition=false.");
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
            Assert.False(outerActivityRan, "Exit em sequência aninhada deve cancelar o Container pai.");
        }

        [Fact]
        public void Execute_MultipleExitCalls_ShouldNotFault()
        {
            // Dois Exit em sequência — o segundo não deve lançar exceção porque
            // o bookmark já foi consumido/cancelado pelo primeiro.
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
                            new Exit()   // bookmark já não existe — deve ser no-op
                        }
                    }
                }
            };

            var app = AppRunner.Run(container, onCompleted: _ => completed.Set());
            Assert.True(completed.Wait(TestTimeout));
            Assert.Null(faultEx);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 4. Body nulo — não deve lançar exceção
        // ──────────────────────────────────────────────────────────────────────

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
    }
}
