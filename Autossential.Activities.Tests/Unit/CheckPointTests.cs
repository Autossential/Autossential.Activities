using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    public class CheckPointTests
    {
        [Fact]
        public void Invoke_WhenExpressionIsTrue_DoesNotThrow()
        {
            var exception = new InvalidOperationException("Test error");

            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = true,
                ["Exception"] = exception
            };

            WorkflowInvoker.Invoke(new CheckPoint(), inputs);
        }

        [Fact]
        public void Invoke_WhenExpressionIsFalse_ThrowsProvidedException()
        {
            var exception = new InvalidOperationException("Expected error");

            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = false,
                ["Exception"] = exception
            };

            var thrown = Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new CheckPoint(), inputs));
            Assert.Equal("Expected error", thrown.Message);
        }

        [Fact]
        public void Invoke_WithDifferentExceptionTypes_ThrowsCorrectException()
        {
            var exception = new ArgumentException("Argument error");

            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = false,
                ["Exception"] = exception
            };

            var thrown = Assert.Throws<ArgumentException>(() => WorkflowInvoker.Invoke(new CheckPoint(), inputs));
            Assert.Equal("Argument error", thrown.Message);
        }

        [Fact]
        public void Invoke_WithOptionalData_DoesNotThrow()
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

        [Fact]
        public void Invoke_WithNullException_ThrowsNullReferenceException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Expression"] = false,
                ["Exception"] = null!
            };

            Assert.Throws<NullReferenceException>(() => WorkflowInvoker.Invoke(new CheckPoint(), inputs));
        }
    }
}
