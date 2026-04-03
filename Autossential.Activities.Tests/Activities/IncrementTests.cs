using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class IncrementTests
    {
        [Fact]
        public void Invoke_DefaultIncrementsByOne()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Variable"] = 1
            };

            var result = WorkflowInvoker.Invoke(new Increment(), inputs);
            // After invocation, InOut arguments are available in inputs
            Assert.Equal(2, result["Variable"]);
        }

        [Fact]
        public void Invoke_CustomValue_IncrementsByValue()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Variable"] = 10,
                ["Value"] = 5
            };

            var result = WorkflowInvoker.Invoke(new Increment(), inputs);
            Assert.Equal(15, result["Variable"]);
        }

        [Fact]
        public void Invoke_NegativeValue_Throws()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Variable"] = 0,
                ["Value"] = 0
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new Increment(), inputs));
        }
    }
}
