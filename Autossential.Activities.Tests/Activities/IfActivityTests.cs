using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Activities.Validation;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class IfActivityTests
    {
        // ─── Helpers ────────────────────────────────────────────────────────────

        /// <summary>
        /// Creates an ActivityFunc&lt;bool&gt; whose Handler always returns the given literal value.
        /// </summary>
        private static ActivityFunc<bool> BoolCondition(bool value) =>
            new ActivityFunc<bool>
            {
                Handler = new Literal<bool>(value)
            };

        /// <summary>
        /// Creates a CodeActivity that appends a token to a shared list,
        /// making it easy to assert execution order / branch selection.
        /// </summary>
        private static Activity RecordActivity(IList<string> log, string token) =>
            new ActionActivity(() => log.Add(token));

        // ─── Constructor / Default State ─────────────────────────────────────

        [Fact]
        public void Constructor_ShouldInitializeConditionAsEmptyActivityFunc()
        {
            var activity = new IfActivity();

            Assert.NotNull(activity.Condition);
            Assert.IsType<ActivityFunc<bool>>(activity.Condition);
            Assert.Null(activity.Condition.Handler);
        }

        [Fact]
        public void Constructor_ShouldInitializeThenAsEmptySequence()
        {
            var activity = new IfActivity();

            Assert.NotNull(activity.Then);
            Assert.IsType<Sequence>(activity.Then);
            Assert.Equal(string.Empty, activity.Then.DisplayName);
        }

        [Fact]
        public void Constructor_ShouldInitializeElseAsEmptySequence()
        {
            var activity = new IfActivity();

            Assert.NotNull(activity.Else);
            Assert.IsType<Sequence>(activity.Else);
            Assert.Equal(string.Empty, activity.Else.DisplayName);
        }

        // ─── Validation ──────────────────────────────────────────────────────

        [Fact]
        public void Validation_ShouldFail_WhenConditionHandlerIsNull()
        {
            var activity = new IfActivity
            {
                // Condition is set but Handler is null (default from constructor)
                Condition = new ActivityFunc<bool>()
            };

            var results = ActivityValidationServices.Validate(activity);

            Assert.NotEmpty(results.Errors);
        }

        [Fact]
        public void Validation_ShouldFail_WhenConditionIsNull()
        {
            var activity = new IfActivity
            {
                Condition = null
            };

            var results = ActivityValidationServices.Validate(activity);

            Assert.NotEmpty(results.Errors);
        }

        [Fact]
        public void Validation_ShouldSucceed_WhenConditionHandlerReturnsBoolean()
        {
            var activity = new IfActivity
            {
                Condition = BoolCondition(true)
            };

            var results = ActivityValidationServices.Validate(activity);

            Assert.Empty(results.Errors);
        }

        // ─── Integration: condition == true ──────────────────────────────────

        [Fact]
        public void Execute_ShouldRunThenBranch_WhenConditionIsTrue()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            Assert.Equal(new[] { "then" }, log);
        }

        [Fact]
        public void Execute_ShouldNotRunElseBranch_WhenConditionIsTrue()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            Assert.DoesNotContain("else", log);
        }

        // ─── Integration: condition == false ─────────────────────────────────

        [Fact]
        public void Execute_ShouldRunElseBranch_WhenConditionIsFalse()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            Assert.Equal(new[] { "else" }, log);
        }

        [Fact]
        public void Execute_ShouldNotRunThenBranch_WhenConditionIsFalse()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            Assert.DoesNotContain("then", log);
        }

        // ─── Integration: null branches ──────────────────────────────────────

        [Fact]
        public void Execute_ShouldNotThrow_WhenThenIsNullAndConditionIsTrue()
        {
            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = null,
                Else = null
            };

            // Must complete without throwing
            var ex = Record.Exception(() => WorkflowInvoker.Invoke(activity));
            Assert.Null(ex);
        }

        [Fact]
        public void Execute_ShouldNotThrow_WhenElseIsNullAndConditionIsFalse()
        {
            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = null,
                Else = null
            };

            var ex = Record.Exception(() => WorkflowInvoker.Invoke(activity));
            Assert.Null(ex);
        }

        [Fact]
        public void Execute_ShouldRunElse_WhenThenIsNullAndConditionIsFalse()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = null,
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            Assert.Equal(new[] { "else" }, log);
        }

        [Fact]
        public void Execute_ShouldRunThen_WhenElseIsNullAndConditionIsTrue()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = RecordActivity(log, "then"),
                Else = null
            };

            WorkflowInvoker.Invoke(activity);

            Assert.Equal(new[] { "then" }, log);
        }

        // ─── Integration: dynamic / runtime condition ─────────────────────────

        [Fact]
        public void Execute_ShouldEvaluateConditionAtRuntime()
        {
            // The condition is backed by a WF variable — value decided at runtime.
            var flag = new Variable<bool>("flag", true);
            var log = new List<string>();

            var activity = new Sequence
            {
                Variables = { flag },
                Activities =
                {
                    new IfActivity
                    {
                        Condition = new ActivityFunc<bool>
                        {
                            Handler = new VariableValue<bool> { Variable = flag }
                        },
                        Then = RecordActivity(log, "runtime-then"),
                        Else = RecordActivity(log, "runtime-else")
                    }
                }
            };

            WorkflowInvoker.Invoke(activity);

            Assert.Equal(new[] { "runtime-then" }, log);
        }

        // ─── Integration: nested IfActivity ──────────────────────────────────

        [Fact]
        public void Execute_ShouldSupportNestedIfActivities()
        {
            var log = new List<string>();

            var inner = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = RecordActivity(log, "inner-then"),
                Else = RecordActivity(log, "inner-else")
            };

            var outer = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = inner,
                Else = RecordActivity(log, "outer-else")
            };

            WorkflowInvoker.Invoke(outer);

            Assert.Equal(new[] { "inner-else" }, log);
        }

        // ─── Integration: Then/Else as Sequence with multiple children ────────

        [Fact]
        public void Execute_ShouldRunAllActivitiesInThenSequence()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = new Sequence
                {
                    Activities =
                    {
                        RecordActivity(log, "step-1"),
                        RecordActivity(log, "step-2"),
                        RecordActivity(log, "step-3")
                    }
                }
            };

            WorkflowInvoker.Invoke(activity);

            Assert.Equal(new[] { "step-1", "step-2", "step-3" }, log);
        }

        [Fact]
        public void Execute_ShouldRunAllActivitiesInElseSequence()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Else = new Sequence
                {
                    Activities =
                    {
                        RecordActivity(log, "else-1"),
                        RecordActivity(log, "else-2")
                    }
                }
            };

            WorkflowInvoker.Invoke(activity);

            Assert.Equal(new[] { "else-1", "else-2" }, log);
        }
    }

    // ─── Minimal test double ──────────────────────────────────────────────────

    /// <summary>
    /// Lightweight <see cref="CodeActivity"/> that executes an arbitrary
    /// <see cref="Action"/> — used to record side-effects in tests without
    /// needing a full mock framework.
    /// </summary>
    internal sealed class ActionActivity : CodeActivity
    {
        private readonly Action _action;

        public ActionActivity(Action action) => _action = action;

        protected override void Execute(CodeActivityContext context) => _action();
    }
}
