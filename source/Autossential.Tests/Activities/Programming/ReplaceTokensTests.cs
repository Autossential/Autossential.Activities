using Autossential.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;

namespace Autossential.Tests
{
    [TestClass]
    public class ReplaceTokensTests
    {
        private const string ExpectedMessage = "The Autossential family libraries are the best =). Lovingly built by Alexandre T. Perez!";

        [TestMethod]
        [DataRow("The {{Library}} family libraries are {{Message}} =). Lovingly built by {{Author}}!", "{{0}}", '0')]
        [DataRow("The %Library family libraries are %Message =). Lovingly built by %Author!", "%0", '0')]
        [DataRow("The 0Library0 family libraries are 0Message0 =). Lovingly built by 0Author0!", "0x0", 'x')]
        [DataRow("The Library# family libraries are Message# =). Lovingly built by Author#!", "0#", '0')]
        [DataRow("The Library family libraries are Message =). Lovingly built by Author!", "0", '0')]
        public void Default(string content, string pattern, char placeholder)
        {
            var workflow = new ReplaceTokens();
            var result = WorkflowInvoker.Invoke(workflow, GetArgs(content, pattern, placeholder));
            Assert.AreEqual(ExpectedMessage, result);
        }

        private static Dictionary<string, object> GetArgs(string content, string pattern, char placeholder)
        {
            return new Dictionary<string, object>
            {
                { nameof(ReplaceTokens.Content), content },
                { nameof(ReplaceTokens.Pattern), pattern },
                { nameof(ReplaceTokens.Placeholder), placeholder },
                { nameof(ReplaceTokens.InputDictionary), new Dictionary<string, object>
                {
                    { "Library", "Autossential" },
                    { "Message", "the best" },
                    { "Author", "Alexandre T. Perez" }
                }}
            };
        }
    }
}