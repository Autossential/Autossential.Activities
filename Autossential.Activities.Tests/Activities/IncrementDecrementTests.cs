using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class IncrementDecrementTests
    {
        [Fact]
        public void Execute_ShouldIncrementVariable()
        {
            // Arrange
            var activity = new IncrementDecrement
            {
                Operation = IncrementDecrement.ArithmeticOperation.Increment
            };

            var inputs = new Dictionary<string, object>
            {
                { "Variable", 10 },
                { "Value", 5 }
            };

            // Act
            var result = WorkflowInvoker.Invoke(activity, inputs);

            // Assert
            Assert.Equal(15, result["Variable"]);
        }

        [Fact]
        public void Execute_ShouldDecrementVariable()
        {
            // Arrange
            var activity = new IncrementDecrement
            {
                Operation = IncrementDecrement.ArithmeticOperation.Decrement
            };

            var inputs = new Dictionary<string, object>
            {
                { "Variable", 10 },
                { "Value", 3 }
            };

            // Act
            var result = WorkflowInvoker.Invoke(activity, inputs);

            // Assert
            Assert.Equal(7, result["Variable"]);
        }

        [Fact]
        public void Execute_ShouldHandleZeroValue()
        {
            // Arrange
            var activity = new IncrementDecrement
            {
                Operation = IncrementDecrement.ArithmeticOperation.Increment
            };

            var inputs = new Dictionary<string, object>
            {
                { "Variable", 10 },
                { "Value", 0 }
            };

            // Act
            var result = WorkflowInvoker.Invoke(activity, inputs);

            // Assert
            Assert.Equal(10, result["Variable"]);
        }

        [Fact]
        public void Execute_ShouldHandleNegativeValue()
        {
            // Arrange
            var activity = new IncrementDecrement
            {
                Operation = IncrementDecrement.ArithmeticOperation.Increment
            };

            var inputs = new Dictionary<string, object>
            {
                { "Variable", 10 },
                { "Value", -5 }
            };

            // Act
            var result = WorkflowInvoker.Invoke(activity, inputs);

            // Assert
            Assert.Equal(5, result["Variable"]);
        }
    }
}
