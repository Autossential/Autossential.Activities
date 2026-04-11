using Autossential.Activities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Autossential.Activities.Tests.Core
{
    public class JsonParserTests
    {
        [Fact]
        public void Parse_ObjectWithScalars_ReturnsDictionary()
        {
            string json = @"{ ""a"": ""text"", ""b"": 123, ""c"": true, ""d"": false, ""e"": null }";
            var result = (Dictionary<string, object>)JsonParser.Parse(json);

            Assert.Equal("text", result["a"]);
            Assert.Equal("123", result["b"]); // number como string
            Assert.Equal("true", result["c"]);
            Assert.Equal("false", result["d"]);
            Assert.Null(result["e"]);
        }

        [Fact]
        public void Parse_ArrayWithMixedTypes_ReturnsList()
        {
            string json = @"[ ""x"", 42, null, true ]";
            var result = (List<object>)JsonParser.Parse(json);

            Assert.Equal(new object[] { "x", "42", null, "true" }, result);
        }

        [Fact]
        public void Parse_NestedObjectAndArray_ReturnsRecursiveStructure()
        {
            string json = @"{ ""arr"": [ { ""k"": ""v"" }, 99 ] }";
            var result = (Dictionary<string, object>)JsonParser.Parse(json);

            var arr = (List<object>)result["arr"];
            var nested = (Dictionary<string, object>)arr[0];

            Assert.Equal("v", nested["k"]);
            Assert.Equal("99", arr[1]);
        }

        [Fact]
        public void Parse_StringScalar_ReturnsString()
        {
            string json = @"""hello""";
            var result = JsonParser.Parse(json);
            Assert.Equal("hello", result);
        }

        [Fact]
        public void Parse_NumberScalar_ReturnsRawText()
        {
            string json = "123.45";
            var result = JsonParser.Parse(json);
            Assert.Equal("123.45", result);
        }

        [Fact]
        public void Parse_BooleanScalars_ReturnsStringTrueFalse()
        {
            Assert.Equal("true", JsonParser.Parse("true"));
            Assert.Equal("false", JsonParser.Parse("false"));
        }

        [Fact]
        public void Parse_NullScalar_ReturnsNull()
        {
            Assert.Null(JsonParser.Parse("null"));
        }

        [Fact]
        public void Parse_WithCommentsAndTrailingCommas_IgnoresThem()
        {
            string json = @"{
            // comentário
            ""a"": [1,2,3,],
        }";
            var result = (Dictionary<string, object>)JsonParser.Parse(json);
            var arr = (List<object>)result["a"];

            Assert.Equal(new[] { "1", "2", "3" }, arr);
        }
    }
}
