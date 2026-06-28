using System.Activities;

namespace Autossential.Activities.Tests.Activities
{
    public class IncrementDecrementTests
    {
        [Test]
        public async Task ShouldIncrementVariable()
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
            await Assert.That(result["Variable"]).IsEqualTo(15);
        }

        [Test]
        public async Task ShouldDecrementVariable()
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
            await Assert.That(result["Variable"]).IsEqualTo(7);
        }

        [Test]
        public async Task ShouldHandleZeroValue()
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
            await Assert.That(result["Variable"]).IsEqualTo(10);
        }

        [Test]
        public async Task ShouldHandleNegativeValue()
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
            await Assert.That(result["Variable"]).IsEqualTo(5);
        }
    }
}