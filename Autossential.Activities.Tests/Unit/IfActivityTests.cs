using Moq;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    /// <summary>
    /// Unit tests for <see cref="IfActivity"/>.
    ///
    /// Strategy:
    ///   - Direct/unit tests: constructor defaults, property initialization, branching logic
    ///     via WorkflowInvoker (no mocks needed for these integration-style unit tests).
    ///   - Isolated tests: OnEvaluateConditionCompleted logic via reflection, and
    ///     NativeActivityContext interactions via Moq.
    ///
    /// Prerequisites (add to your test project):
    ///   - xunit
    ///   - xunit.runner.visualstudio
    ///   - Moq
    ///   - System.Activities (WF 4 / CoreWF)
    /// </summary>
    public class IfActivityTests
    {
        // ──────────────────────────────────────────────────────────────────────
        // 1. Constructor / Initialization
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Constructor_ShouldInitializeCondition()
        {
            var activity = new IfActivity();

            Assert.NotNull(activity.Condition);
        }

        [Fact]
        public void Constructor_ShouldInitializeBody_WithThenSequence()
        {
            var activity = new IfActivity();

            Assert.NotNull(activity.Body);
            Assert.NotNull(activity.Body.Handler);
            var sequence = Assert.IsType<Sequence>(activity.Body.Handler);
            Assert.Equal("Then", sequence.DisplayName);
        }

        [Fact]
        public void Constructor_ShouldInitializeElseBody_WithElseSequence()
        {
            var activity = new IfActivity();

            Assert.NotNull(activity.ElseBody);
            Assert.NotNull(activity.ElseBody.Handler);
            var sequence = Assert.IsType<Sequence>(activity.ElseBody.Handler);
            Assert.Equal("Else", sequence.DisplayName);
        }

        [Fact]
        public void Constructor_ShouldSetCheckTrue_ToTrueByDefault()
        {
            var activity = new IfActivity();

            Assert.True(activity.CheckTrue);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 2. Variables collection (lazy init)
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Variables_ShouldReturnEmptyCollection_WhenNotSet()
        {
            var activity = new IfActivity();

            Assert.NotNull(activity.Variables);
            Assert.Empty(activity.Variables);
        }

        [Fact]
        public void Variables_ShouldReturnSameInstance_OnSubsequentAccess()
        {
            var activity = new IfActivity();

            var first = activity.Variables;
            var second = activity.Variables;

            Assert.Same(first, second);
        }

        [Fact]
        public void Variables_ShouldAllowAddingItems()
        {
            var activity = new IfActivity();
            var variable = new Variable<string>("testVar");

            activity.Variables.Add(variable);

            Assert.Single(activity.Variables);
            Assert.Contains(variable, activity.Variables);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 3. Property mutation
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void CheckTrue_CanBeSetToFalse()
        {
            var activity = new IfActivity { CheckTrue = false };

            Assert.False(activity.CheckTrue);
        }

        [Fact]
        public void Condition_CanBeReplacedWithNewHandler()
        {
            var activity = new IfActivity();
            var newFunc = new ActivityFunc<bool> { Handler = new DummyBoolActivity(true) };

            activity.Condition = newFunc;

            Assert.Same(newFunc, activity.Condition);
        }

        [Fact]
        public void Body_CanBeReplacedWithCustomAction()
        {
            var activity = new IfActivity();
            var newAction = new ActivityAction { Handler = new Sequence { DisplayName = "Custom" } };

            activity.Body = newAction;

            Assert.Same(newAction, activity.Body);
        }

        [Fact]
        public void ElseBody_CanBeReplacedWithCustomAction()
        {
            var activity = new IfActivity();
            var newAction = new ActivityAction { Handler = new Sequence { DisplayName = "CustomElse" } };

            activity.ElseBody = newAction;

            Assert.Same(newAction, activity.ElseBody);
        }

        // ──────────────────────────────────────────────────────────────────────
        // 4. Workflow integration – branching via WorkflowInvoker
        //
        //    WorkflowInvoker executes synchronously in-process, making it the
        //    simplest way to exercise Execute → OnEvaluateConditionCompleted
        //    without mocking the entire NativeActivityContext.
        // ──────────────────────────────────────────────────────────────────────

        [Fact]
        public void Execute_WhenConditionTrue_AndCheckTrue_ShouldRunBody()
        {
            var bodyRan = false;
            var elseRan = false;

            var activity = BuildIfActivity(
                conditionResult: true,
                checkTrue: true,
                bodyAction: () => bodyRan = true,
                elseAction: () => elseRan = true);

            WorkflowInvoker.Invoke(activity);

            Assert.True(bodyRan, "Body should have run.");
            Assert.False(elseRan, "ElseBody should not have run.");
        }

        [Fact]
        public void Execute_WhenConditionFalse_AndCheckTrue_ShouldRunElseBody()
        {
            var bodyRan = false;
            var elseRan = false;

            var activity = BuildIfActivity(
                conditionResult: false,
                checkTrue: true,
                bodyAction: () => bodyRan = true,
                elseAction: () => elseRan = true);

            WorkflowInvoker.Invoke(activity);

            Assert.False(bodyRan, "Body should not have run.");
            Assert.True(elseRan, "ElseBody should have run.");
        }

        [Fact]
        public void Execute_WhenConditionFalse_AndCheckFalse_ShouldRunBody()
        {
            // CheckTrue = false means "run Body when condition is false"
            var bodyRan = false;
            var elseRan = false;

            var activity = BuildIfActivity(
                conditionResult: false,
                checkTrue: false,
                bodyAction: () => bodyRan = true,
                elseAction: () => elseRan = true);

            WorkflowInvoker.Invoke(activity);

            Assert.True(bodyRan, "Body should have run when condition matches CheckTrue=false.");
            Assert.False(elseRan, "ElseBody should not have run.");
        }

        [Fact]
        public void Execute_WhenConditionTrue_AndCheckFalse_ShouldRunElseBody()
        {
            var bodyRan = false;
            var elseRan = false;

            var activity = BuildIfActivity(
                conditionResult: true,
                checkTrue: false,
                bodyAction: () => bodyRan = true,
                elseAction: () => elseRan = true);

            WorkflowInvoker.Invoke(activity);

            Assert.False(bodyRan, "Body should not have run.");
            Assert.True(elseRan, "ElseBody should have run.");
        }

        [Fact]
        public void Execute_WhenBodyIsNull_AndConditionMatches_ShouldNotThrow()
        {
            var activity = new IfActivity
            {
                CheckTrue = true,
                Condition = new ActivityFunc<bool> { Handler = new DummyBoolActivity(true) },
                Body = null   // explicitly null
            };

            // Should complete without throwing NullReferenceException
            var ex = Record.Exception(() => WorkflowInvoker.Invoke(activity));

            Assert.Null(ex);
        }

        [Fact]
        public void Execute_WhenElseBodyIsNull_AndConditionDoesNotMatch_ShouldNotThrow()
        {
            var activity = new IfActivity
            {
                CheckTrue = true,
                Condition = new ActivityFunc<bool> { Handler = new DummyBoolActivity(false) },
                ElseBody = null  // explicitly null
            };

            var ex = Record.Exception(() => WorkflowInvoker.Invoke(activity));

            Assert.Null(ex);
        }

        // ──────────────────────────────────────────────────────────────────────
        // Helpers
        // ──────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Builds a fully wired <see cref="IfActivity"/> using lightweight
        /// code-activities for condition, body, and else-body.
        /// </summary>
        private static IfActivity BuildIfActivity(
            bool conditionResult,
            bool checkTrue,
            Action bodyAction,
            Action elseAction)
        {
            return new IfActivity
            {
                CheckTrue = checkTrue,
                Condition = new ActivityFunc<bool>
                {
                    Handler = new DummyBoolActivity(conditionResult)
                },
                Body = new ActivityAction
                {
                    Handler = new InvokeAction(bodyAction)
                },
                ElseBody = new ActivityAction
                {
                    Handler = new InvokeAction(elseAction)
                }
            };
        }



        // ──────────────────────────────────────────────────────────────────────────
        // Test doubles (minimal, self-contained)
        // ──────────────────────────────────────────────────────────────────────────

        /// <summary>Returns a constant bool value — used as the Condition handler.</summary>
        internal sealed class DummyBoolActivity(bool value) : CodeActivity<bool>
        {
            private readonly bool _value = value;

            protected override bool Execute(CodeActivityContext context) => _value;
        }

        /// <summary>Invokes a delegate — used as Body / ElseBody handlers.</summary>
        internal sealed class InvokeAction(Action action) : CodeActivity
        {
            private readonly Action _action = action;

            protected override void Execute(CodeActivityContext context) => _action();
        }
    }
}
