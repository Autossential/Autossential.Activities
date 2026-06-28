using Autossential.Activities.Tests.Helpers;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Activities.Validation;

namespace Autossential.Activities.Tests.Activities
{
    public class IfActivityTests
    {
        /// <summary>
        /// Creates an ActivityFunc&lt;bool&gt; whose Handler always returns the given literal value.
        /// </summary>
        private static ActivityFunc<bool> BoolCondition(bool value) =>
            new() { Handler = new Literal<bool>(value) };

        /// <summary>
        /// Creates a CodeActivity that appends a token to a shared list,
        /// making it easy to assert execution order / branch selection.
        /// </summary>
        private static Activity RecordActivity(IList<string> log, string token) =>
            new ActionInvoker(() => log.Add(token));

        [Test]
        public async Task ShouldInitializeConditionAsEmptyActivityFunc()
        {
            var activity = new IfActivity();

            await Assert.That(activity.Condition).IsNotNull();
            await Assert.That(activity.Condition).IsTypeOf<ActivityFunc<bool>>();
            await Assert.That(activity.Condition.Handler).IsNull();
        }

        [Test]
        public async Task ShouldInitializeThenAsEmptySequence()
        {
            var activity = new IfActivity();

            await Assert.That(activity.Then).IsNotNull();
            await Assert.That(activity.Then).IsTypeOf<Sequence>();
            await Assert.That(activity.Then.DisplayName).IsEqualTo(string.Empty);
        }

        [Test]
        public async Task ShouldInitializeElseAsEmptySequence()
        {
            var activity = new IfActivity();

            await Assert.That(activity.Else).IsNotNull();
            await Assert.That(activity.Else).IsTypeOf<Sequence>();
            await Assert.That(activity.Else.DisplayName).IsEqualTo(string.Empty);
        }

        [Test]
        public async Task Validation_ShouldFail_WhenConditionHandlerIsNull()
        {
            var activity = new IfActivity
            {
                // Condition is set but Handler is null (default from constructor)
                Condition = new ActivityFunc<bool>()
            };

            var results = ActivityValidationServices.Validate(activity);

            await Assert.That(results.Errors).IsNotEmpty();
        }

        [Test]
        public async Task Validation_ShouldFail_WhenConditionIsNull()
        {
            var activity = new IfActivity
            {
                Condition = null
            };

            var results = ActivityValidationServices.Validate(activity);

            await Assert.That(results.Errors).IsNotEmpty();
        }

        [Test]
        public async Task Validation_ShouldSucceed_WhenConditionHandlerReturnsBoolean()
        {
            var activity = new IfActivity
            {
                Condition = BoolCondition(true)
            };

            var results = ActivityValidationServices.Validate(activity);

            await Assert.That(results.Errors).IsEmpty();
        }

        // ─── Integration: condition == true ──────────────────────────────────

        [Test]
        public async Task ShouldRunThenBranch_WhenConditionIsTrue()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            await Assert.That(log).IsEquivalentTo(new[] { "then" });
        }

        [Test]
        public async Task ShouldNotRunElseBranch_WhenConditionIsTrue()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            await Assert.That(log).DoesNotContain("else");
        }

        // ─── Integration: condition == false ─────────────────────────────────

        [Test]
        public async Task ShouldRunElseBranch_WhenConditionIsFalse()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            await Assert.That(log).IsEquivalentTo(new[] { "else" });
        }

        [Test]
        public async Task ShouldNotRunThenBranch_WhenConditionIsFalse()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = RecordActivity(log, "then"),
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            await Assert.That(log).DoesNotContain("then");
        }

        // ─── Integration: null branches ──────────────────────────────────────

        [Test]
        public void ShouldNotThrow_WhenThenIsNullAndConditionIsTrue()
        {
            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = null,
                Else = null
            };

            WorkflowInvoker.Invoke(activity);
        }

        [Test]
        public void ShouldNotThrow_WhenElseIsNullAndConditionIsFalse()
        {
            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = null,
                Else = null
            };

            WorkflowInvoker.Invoke(activity);
        }

        [Test]
        public async Task ShouldRunElse_WhenThenIsNullAndConditionIsFalse()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(false),
                Then = null,
                Else = RecordActivity(log, "else")
            };

            WorkflowInvoker.Invoke(activity);

            await Assert.That(log).IsEquivalentTo(new[] { "else" });
        }

        [Test]
        public async Task ShouldRunThen_WhenElseIsNullAndConditionIsTrue()
        {
            var log = new List<string>();

            var activity = new IfActivity
            {
                Condition = BoolCondition(true),
                Then = RecordActivity(log, "then"),
                Else = null
            };

            WorkflowInvoker.Invoke(activity);

            await Assert.That(log).IsEquivalentTo(new[] { "then" });
        }

        // ─── Integration: dynamic / runtime condition ─────────────────────────

        [Test]
        public async Task ShouldEvaluateConditionAtRuntime()
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

            await Assert.That(log).IsEquivalentTo(new[] { "runtime-then" });
        }

        // ─── Integration: nested IfActivity ──────────────────────────────────

        [Test]
        public async Task ShouldSupportNestedIfActivities()
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

            await Assert.That(log).IsEquivalentTo(new[] { "inner-else" });
        }

        // ─── Integration: Then/Else as Sequence with multiple children ────────

        [Test]
        public async Task ShouldRunAllActivitiesInThenSequence()
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

            await Assert.That(log).IsEquivalentTo(new[] { "step-1", "step-2", "step-3" });
        }

        [Test]
        public async Task ShouldRunAllActivitiesInElseSequence()
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

            await Assert.That(log).IsEquivalentTo(new[] { "else-1", "else-2" });
        }
    }
}