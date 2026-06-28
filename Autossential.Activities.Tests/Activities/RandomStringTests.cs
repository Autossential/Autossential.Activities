using System.Activities;

namespace Autossential.Activities.Tests.Activities
{
    public class RandomStringTests
    {
        [Test]
        public async Task DefaultFormat_ReturnsLength8()
        {
            var result = WorkflowInvoker.Invoke(new RandomString());
            await Assert.That(result).IsNotNull();
            await Assert.That(result.Length).IsEqualTo(8);
        }

        [Test]
        public async Task CustomCharacters_OnlyUsesCustom()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Format"] = "????",
                ["Custom"] = "XYZ"
            };

            var result = WorkflowInvoker.Invoke(new RandomString(), inputs);

            await Assert.That(result.Length).IsEqualTo(4);
            foreach (var c in result)
                await Assert.That("XYZ").Contains(c.ToString());
        }

        [Test]
        public async Task EscapeCharacter_PreservesNext()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Format"] = "\\A"
            };

            var result = WorkflowInvoker.Invoke(new RandomString(), inputs);

            await Assert.That(result).IsEqualTo("A");
        }

        [Test]
        public async Task EmptyFormat_ThrowsInvalidOperationException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Format"] = string.Empty
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new RandomString(), inputs))
                .Throws<InvalidOperationException>();
        }
    }
}