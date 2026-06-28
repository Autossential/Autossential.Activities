using Autossential.Activities.Models;
using System.Activities;
using System.Globalization;

namespace Autossential.Activities.Tests.Activities
{
    public class ParseDataTests
    {
        [Test]
        public async Task WithJsonContent_ParsesAsJson()
        {
            var jsonContent = "{\"name\": \"Alice\", \"age\": 30}";

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = jsonContent
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            await Assert.That(result).IsNotNull();
            await Assert.That(result["name"].AsString()).IsEqualTo("Alice");
            await Assert.That(result["age"].AsString()).IsEqualTo("30");
        }

        [Test]
        public async Task WithJsonArray_ParsesAsJsonArray()
        {
            var jsonContent = "[\"item1\", \"item2\", \"item3\"]";

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = jsonContent
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task WithYamlContent_ParsesAsYaml()
        {
            var yamlContent = "name: Bob\nage: 25";

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = yamlContent
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task WithEmptyContent_ReturnsNull()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Content"] = string.Empty
            };

            var result = WorkflowInvoker.Invoke(new ParseData(), inputs);

            await Assert.That(result).IsNull();
        }

        [Test]
        public async Task WithNullContent_ReturnsNull()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Content"] = null!
            };

            var result = WorkflowInvoker.Invoke(new ParseData(), inputs);

            await Assert.That(result).IsNull();
        }

        [Test]
        public async Task WithCustomCulture_UsesCultureForParsing()
        {
            var jsonContent = "{\"value\": \"123,45\"}";
            var culture = new CultureInfo("pt-BR");

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = jsonContent,
                ["Culture"] = culture
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            await Assert.That(result).IsNotNull();
        }
    }
}