using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class DecrementTests
    {
        [Fact]
        public void Invoke_DefaultDecrementsByOne()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Variable"] = 5
            };

            var result = WorkflowInvoker.Invoke(new Autossential.Activities.Decrement(), inputs);
            Assert.Equal(4, result["Variable"]);
        }

        [Fact]
        public void Invoke_CustomValue_DecrementsByValue()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Variable"] = 10,
                ["Value"] = 3
            };

            var result = WorkflowInvoker.Invoke(new Autossential.Activities.Decrement(), inputs);
            Assert.Equal(7, result["Variable"]);
        }

        [Fact]
        public void Invoke_ZeroValue_Throws()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Variable"] = 0,
                ["Value"] = 0
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new Autossential.Activities.Decrement(), inputs));
        }
    }
}
