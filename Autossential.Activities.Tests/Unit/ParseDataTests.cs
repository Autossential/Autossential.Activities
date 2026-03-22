using System;
using System.Activities;
using System.Collections.Generic;
using System.Globalization;
using Autossential.Activities.Models;
using Xunit;

namespace Autossential.Activities.Tests.Unit
{
    public class ParseDataTests
    {
        [Fact]
        public void Invoke_WithJsonContent_ParsesAsJson()
        {
            var jsonContent = "{\"name\": \"Alice\", \"age\": 30}";

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = jsonContent
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            Assert.NotNull(result);
            Assert.Equal("Alice", result["name"].AsString());
            Assert.Equal("30", result["age"].AsString());
        }

        [Fact]
        public void Invoke_WithJsonArray_ParsesAsJsonArray()
        {
            var jsonContent = "[\"item1\", \"item2\", \"item3\"]";

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = jsonContent
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            Assert.NotNull(result);
        }

        [Fact]
        public void Invoke_WithYamlContent_ParsesAsYaml()
        {
            var yamlContent = "name: Bob\nage: 25";

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = yamlContent
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            Assert.NotNull(result);
        }

        [Fact]
        public void Invoke_WithEmptyContent_ReturnsNull()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Content"] = string.Empty
            };

            var result = WorkflowInvoker.Invoke(new ParseData(), inputs);

            Assert.Null(result);
        }

        [Fact]
        public void Invoke_WithNullContent_ReturnsNull()
        {
            var inputs = new Dictionary<string, object>
            {
                ["Content"] = null!
            };

            var result = WorkflowInvoker.Invoke(new ParseData(), inputs);

            Assert.Null(result);
        }

        [Fact]
        public void Invoke_WithCustomCulture_UsesCultureForParsing()
        {
            var jsonContent = "{\"value\": \"123,45\"}";
            var culture = new CultureInfo("pt-BR");

            var inputs = new Dictionary<string, object>
            {
                ["Content"] = jsonContent,
                ["Culture"] = culture
            };

            var result = (DataNode)WorkflowInvoker.Invoke(new ParseData(), inputs);

            Assert.NotNull(result);
        }
    }
}
