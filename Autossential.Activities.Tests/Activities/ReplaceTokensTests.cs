using System;
using System.Activities;
using System.Collections.Generic;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class ReplaceTokensTests
    {
        [Fact]
        public void Invoke_WithSimpleTokens_ReplacesTokensInContent()
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

            var result = (string)WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);

            Assert.Equal("Hello Alice, you are 30 years old", result);
        }

        [Fact]
        public void Invoke_WithCustomPattern_ReplacesWithCustomPattern()
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

            var result = (string)WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);

            Assert.Equal("User: Bob, Role: Admin", result);
        }

        [Fact]
        public void Invoke_WithCaseSensitiveTrue_MatchesExactCase()
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

            var result = (string)WorkflowInvoker.Invoke(new ReplaceTokens(), inputs);

            // Only {{Name}} should be replaced
            Assert.Contains("{{name}}", result);
            Assert.Contains("Charlie", result);
        }

        [Fact]
        public void Invoke_WithNullContent_ReturnsNull()
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

            Assert.Null(result);
        }
    }
}
