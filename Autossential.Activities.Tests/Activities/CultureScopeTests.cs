using Autossential.Activities.Tests.Helpers;
using System.Activities;
using System.Activities.Statements;
using System.Globalization;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class CultureScopeTests
    {
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(10);

        // ──────────────────────────────────────────────────────────────────────
        // 1. Constructor / Initialization
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Constructor_ShouldInitializeBody_WithDoSequence()
        {
            var scope = new CultureScope();

            Assert.NotNull(scope.Body);
            Assert.NotNull(scope.Body.Handler);
            var sequence = Assert.IsType<Sequence>(scope.Body.Handler);
            Assert.Equal("Do", sequence.DisplayName);
        }

        [Fact]
        public void Constructor_Body_ShouldBeMutable()
        {
            var scope = new CultureScope();
            var custom = new ActivityAction { Handler = new Sequence { DisplayName = "Custom" } };

            scope.Body = custom;

            Assert.Same(custom, scope.Body);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 2. Culture aplicada durante execução do Body
        // ──────────────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("en-US")]
        [InlineData("pt-BR")]
        [InlineData("fr-FR")]
        [InlineData("de-DE")]
        [InlineData("ja-JP")]
        public void Execute_ShouldApplyCulture_DuringBodyExecution(string cultureName)
        {
            CultureInfo? cultureInsideBody = null;
            var completed = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>(cultureName),
                Body = new ActivityAction
                {
                    Handler = new ActionInvoker(() => cultureInsideBody = CultureInfo.CurrentCulture)
                }
            };

            AppRunner.Run(scope, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.NotNull(cultureInsideBody);
            Assert.Equal(cultureName, cultureInsideBody!.Name);
        }

        [Fact]
        public void Execute_ShouldApplyCulture_DifferentFromOriginal()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var targetCulture = originalCulture.Name == "en-US" ? "pt-BR" : "en-US";
            CultureInfo? inside = null;
            var completed = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>(targetCulture),
                Body = new ActivityAction
                {
                    Handler = new ActionInvoker(() => inside = CultureInfo.CurrentCulture)
                }
            };

            AppRunner.Run(scope, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.Equal(targetCulture, inside!.Name);
            Assert.NotEqual(originalCulture.Name, inside.Name);
        }

        [Fact]
        public void Execute_WithEmptyCultureString_ShouldUseInvariantCulture()
        {
            CultureInfo? inside = null;
            var completed = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>(""),
                Body = new ActivityAction
                {
                    Handler = new ActionInvoker(() => inside = CultureInfo.CurrentCulture)
                }
            };

            AppRunner.Run(scope, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.Equal(CultureInfo.InvariantCulture.Name, inside!.Name);
        }

        [Fact]
        public void Execute_WithNullCulture_ShouldFallbackToEmptyString_AndUseInvariantCulture()
        {
            CultureInfo? inside = null;
            var completed = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>((string)null),
                Body = new ActivityAction
                {
                    Handler = new ActionInvoker(() => inside = CultureInfo.CurrentCulture)
                }
            };

            AppRunner.Run(scope, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.Equal(CultureInfo.InvariantCulture.Name, inside!.Name);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 3. Culture restaurada após execução
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_ShouldRestoreOriginalCulture_AfterBodyCompletes()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            CultureInfo? after = null;
            var completed = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>("ja-JP"),
                Body = new ActivityAction { Handler = new Sequence() }
            };

            AppRunner.Run(scope, onCompleted: _ =>
            {
                after = CultureInfo.CurrentCulture;
                completed.Set();
            });

            Assert.True(completed.Wait(TestTimeout));
            Assert.Equal(originalCulture.Name, after!.Name);
        }

        [Fact]
        public void Execute_NestedCultureScopes_ShouldApplyInnerCulture_ThenRestoreOuter()
        {
            CultureInfo? innerCulture = null;
            CultureInfo? outerCulture = null;
            var completed = new ManualResetEventSlim(false);

            var outer = new CultureScope
            {
                Culture = new InArgument<string>("en-US"),
                Body = new ActivityAction
                {
                    Handler = new Sequence
                    {
                        Activities =
                        {
                            new CultureScope
                            {
                                Culture = new InArgument<string>("fr-FR"),
                                Body    = new ActivityAction
                                {
                                    Handler = new ActionInvoker(() => innerCulture = CultureInfo.CurrentCulture)
                                }
                            },
                            new ActionInvoker(() => outerCulture = CultureInfo.CurrentCulture)
                        }
                    }
                }
            };

            AppRunner.Run(outer, onCompleted: _ => completed.Set());

            Assert.True(completed.Wait(TestTimeout));
            Assert.Equal("fr-FR", innerCulture!.Name);
            Assert.Equal("en-US", outerCulture!.Name);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 4. Culture restaurada após fault
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_ShouldRestoreOriginalCulture_AfterBodyFaults()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            CultureInfo? after = null;
            var signaled = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>("de-DE"),
                Body = new ActivityAction
                {
                    Handler = new ActionInvoker(() => throw new ApplicationException("Boom!"))
                }
            };

            AppRunner.Run(scope, onCompleted: _ =>
            {
                after = CultureInfo.CurrentCulture;
                signaled.Set();
            });

            Assert.True(signaled.Wait(TestTimeout));
            Assert.Equal(originalCulture.Name, after!.Name);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 5. Culture inválida — deve completar com Faulted
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_WithInvalidCultureName_ShouldCompleteWithFaultedState()
        {
            WorkflowApplicationCompletedEventArgs? completedArgs = null;
            var signaled = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>("invalid-XX"),
                Body = new ActivityAction { Handler = new Sequence() }
            };

            AppRunner.Run(scope, onCompleted: args =>
            {
                completedArgs = args;
                signaled.Set();
            });

            Assert.True(signaled.Wait(TestTimeout));
            Assert.Equal(ActivityInstanceState.Faulted, completedArgs!.CompletionState);
            Assert.IsType<CultureNotFoundException>(completedArgs.TerminationException);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 6. Body nulo
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_WhenBodyIsNull_ShouldCompleteWithoutFault()
        {
            WorkflowApplicationCompletedEventArgs? completedArgs = null;
            var completed = new ManualResetEventSlim(false);

            var scope = new CultureScope
            {
                Culture = new InArgument<string>("en-US"),
                Body = null
            };

            AppRunner.Run(scope, onCompleted: args =>
            {
                completedArgs = args;
                completed.Set();
            });

            Assert.True(completed.Wait(TestTimeout));
            Assert.Equal(ActivityInstanceState.Closed, completedArgs!.CompletionState);
        }
    }
}