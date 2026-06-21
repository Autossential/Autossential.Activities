using System.Activities;

namespace Autossential.Activities.Tests.Activities
{
    public class CheckPointTests
    {
        [Test]
        public void DoesNotThrow_WhenExpressionIsTrue()
        {
            var exception = new InvalidOperationException("Test error");

            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = true,
                ["Exception"] = exception
            };

            WorkflowInvoker.Invoke(new CheckPoint(), inputs);
        }

        [Test]
        public void DoesNotThrow_WithOptionalData()
        {
            var exception = new InvalidOperationException("Test");
            var data = new Dictionary<string, string> { ["key"] = "value" };

            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = true,
                ["Exception"] = exception,
                ["Data"] = data
            };

            WorkflowInvoker.Invoke(new CheckPoint(), inputs);
        }

        [Test]
        public async Task ThrowsProvidedException_WhenExpressionIsFalse()
        {
            var exception = new InvalidOperationException("Expected error");

            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = false,
                ["Exception"] = exception
            };

            var thrown = await Assert.That(() => WorkflowInvoker.Invoke(new CheckPoint(), inputs))
                .Throws<InvalidOperationException>();

            await Assert.That(thrown?.Message).IsEqualTo("Expected error");
        }

        [Test]
        public async Task ThrowsCorrectException_WithDifferentExceptionTypes()
        {
            var exception = new ArgumentException("Argument error");

            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = false,
                ["Exception"] = exception
            };

            var thrown = await Assert.That(() => WorkflowInvoker.Invoke(new CheckPoint(), inputs))
                .Throws<ArgumentException>();

            await Assert.That(thrown?.Message).IsEqualTo("Argument error");
        }

        [Test]
        public async Task ThrowsNullReferenceException_WithNullException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = false,
                ["Exception"] = null!
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new CheckPoint(), inputs))
                .Throws<NullReferenceException>();
        }
    }
}