using System.Activities;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class RandomStringTests
    {
        [Fact]
        public void Invoke_DefaultFormat_ReturnsLength8()
        {
            var result = WorkflowInvoker.Invoke(new RandomString());
            Assert.NotNull(result);
            Assert.Equal(8, result.Length);
        }

        [Fact]
        public void Invoke_CustomCharacters_OnlyUsesCustom()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Format"] = "????",
                ["Custom"] = "XYZ"
            };

            var result = WorkflowInvoker.Invoke(new RandomString(), inputs);

            Assert.Equal(4, result.Length);
            foreach (var c in result)
                Assert.Contains(c, "XYZ");
        }

        [Fact]
        public void Invoke_EscapeCharacter_PreservesNext()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Format"] = "\\A"
            };

            var result = WorkflowInvoker.Invoke(new RandomString(), inputs);

            Assert.Equal("A", result);
        }

        [Fact]
        public void Invoke_EmptyFormat_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Format"] = string.Empty
            };

            Assert.Throws<InvalidOperationException>(() => WorkflowInvoker.Invoke(new RandomString(), inputs));
        }
    }
}
