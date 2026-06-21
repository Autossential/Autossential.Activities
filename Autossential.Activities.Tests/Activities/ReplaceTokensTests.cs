using System;
using System.Activities;
using System.Collections.Generic;
using TUnit;

namespace Autossential.Activities.Tests.Activities
{
    public class ReplaceTokensTests
    {
        [Test]
        public async Task WithSimpleTokens_ReplacesTokensInContent()
        {
            var dictionary = new Dictionary<string, object>
            {
                ["name"] = "Alice",
                ["age"] = 30
            };

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = "Hello {{name}}, you are {{age}} years old",
                ["Dictionary"] = dictionary,
                ["Pattern"] = "{{0}}",
                ["Placeholder"] = '0'
            };

            var result = WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);

            await Assert.That(result).IsEqualTo("Hello Alice, you are 30 years old");
        }

        [Test]
        public async Task WithCustomPattern_ReplacesWithCustomPattern()
        {
            var dictionary = new Dictionary<string, object>
            {
                ["user"] = "Bob",
                ["role"] = "Admin"
            };

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = "User: [user], Role: [role]",
                ["Dictionary"] = dictionary,
                ["Pattern"] = "[0]",
                ["Placeholder"] = '0'
            };

            var result = WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);

            await Assert.That(result).IsEqualTo("User: Bob, Role: Admin");
        }

        [Test]
        public async Task WithCaseSensitiveTrue_MatchesExactCase()
        {
            var dictionary = new Dictionary<string, object>
            {
                ["Name"] = "Charlie"
            };

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = "{{name}} and {{Name}}",
                ["Dictionary"] = dictionary,
                ["Pattern"] = "{{0}}",
                ["Placeholder"] = '0'
            };

            var result = WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);

            // Only {{Name}} should be replaced
            await Assert.That(result).Contains("{{name}}");
            await Assert.That(result).Contains("Charlie");
        }

        [Test]
        public async Task WithNullContent_ReturnsNull()
        {
            var dictionary = new Dictionary<string, object>
            {
                ["key"] = "value"
            };

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = null!,
                ["Dictionary"] = dictionary,
                ["Pattern"] = "{{0}}",
                ["Placeholder"] = '0'
            };

            var result = WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);

            await Assert.That(result).IsNull();
        }

        [Test]
        [Arguments("%0", '0')]
        [Arguments("@*", '@')]
        [Arguments("__~", '~')]
        public async Task UnusualPatterns_ShouldWork(string pattern, char placeholder)
        {
            var dictionary = new Dictionary<string, object>
            {
                ["Name"] = "Alice"
            };

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = string.Format("Hi, my name is {0}", pattern.Replace(placeholder.ToString(), "Name")),
                ["Dictionary"] = dictionary,
                ["Pattern"] = pattern,
                ["Placeholder"] = placeholder
            };

            var result = WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);
            await Assert.That(result).Contains("Alice");
        }
    }
}