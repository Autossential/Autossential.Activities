using Autossential.Activities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security;
using UiPath.Studio.Activities.Api;
using Xunit;

namespace Autossential.Activities.Tests.Core
{
    public class DataNodeTests
    {
        // ─────────────────────────────────────────────
        // Construction & Type inference
        // ─────────────────────────────────────────────

        [Fact]
        public void Constructor_WithNull_IsScalarWithNoValue()
        {
            var node = new DataNode((object)null);
            Assert.Equal(NodeType.Scalar, node.Type);
            Assert.False(node.HasValue);
            Assert.Null(node.RawValue);
        }

        [Fact]
        public void Constructor_WithString_IsScalar()
        {
            var node = new DataNode("hello");
            Assert.Equal(NodeType.Scalar, node.Type);
            Assert.True(node.HasValue);
            Assert.Equal("hello", node.RawValue);
        }

        [Fact]
        public void Constructor_WithInt_IsScalar()
        {
            var node = new DataNode(42);
            Assert.Equal(NodeType.Scalar, node.Type);
            Assert.True(node.HasValue);
        }

        [Fact]
        public void Constructor_WithDictionary_IsMap()
        {
            var dict = new Dictionary<string, object> { { "key", "value" } };
            var node = new DataNode(dict);
            Assert.Equal(NodeType.Map, node.Type);
        }

        [Fact]
        public void Constructor_WithList_IsSequence()
        {
            var list = new List<object> { "a", "b" };
            var node = new DataNode(list);
            Assert.Equal(NodeType.Sequence, node.Type);
        }

        [Fact]
        public void Constructor_WithNestedDataNode_Unwraps()
        {
            var inner = new DataNode("unwrapped");
            var outer = new DataNode(inner);
            Assert.Equal(NodeType.Scalar, outer.Type);
            Assert.Equal("unwrapped", outer.RawValue);
        }

        [Fact]
        public void DefaultConstructor_CreatesEmptyMap()
        {
            var node = new DataNode();
            Assert.Equal(NodeType.Map, node.Type);
        }

        [Fact]
        public void Constructor_WithCulture_StoresCulture()
        {
            var culture = new CultureInfo("pt-BR");
            var node = new DataNode("val", culture);
            Assert.Equal(culture, node.Culture);
        }

        [Fact]
        public void Constructor_WithoutCulture_UsesInvariantCulture()
        {
            var node = new DataNode("val");
            Assert.Equal(CultureInfo.InvariantCulture, node.Culture);
        }

        // ─────────────────────────────────────────────
        // Normalization
        // ─────────────────────────────────────────────

        [Fact]
        public void Normalize_NonGenericDictionary_ConvertsToDictionaryOfStringObject()
        {
            var raw = new System.Collections.Hashtable { { "x", 1 } };
            var node = new DataNode(raw);
            Assert.Equal(NodeType.Map, node.Type);
            Assert.IsType<Dictionary<string, object>>(node.RawValue);
        }

        [Fact]
        public void Normalize_Array_ConvertsToListOfObject()
        {
            var node = new DataNode(new[] { "a", "b", "c" });
            Assert.Equal(NodeType.Sequence, node.Type);
            Assert.IsType<List<object>>(node.RawValue);
        }

        [Fact]
        public void Normalize_NestedDictInList_IsNormalized()
        {
            var nested = new Dictionary<string, object> { { "k", "v" } };
            var node = new DataNode(new List<object> { nested });
            var seq = node.AsSequence();
            Assert.IsType<Dictionary<string, object>>(seq[0]);
        }

        // ─────────────────────────────────────────────
        // GetNode – dot notation
        // ─────────────────────────────────────────────

        [Fact]
        public void GetNode_SimpleKey_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object> { { "name", "Alex" } });
            Assert.Equal("Alex", node.GetNode("name").AsString());
        }

        [Fact]
        public void GetNode_NestedDotPath_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "person", new Dictionary<string, object> { { "city", "Curitiba" } } }
            });
            Assert.Equal("Curitiba", node.GetNode("person.city").AsString());
        }

        [Fact]
        public void GetNode_MissingKey_ReturnsNullScalar()
        {
            var node = new DataNode(new Dictionary<string, object>());
            var result = node.GetNode("missing");
            Assert.False(result.HasValue);
        }

        [Fact]
        public void GetNode_MissingIntermediateKey_ReturnsNullScalar()
        {
            var node = new DataNode(new Dictionary<string, object>());
            var result = node.GetNode("a.b.c");
            Assert.False(result.HasValue);
        }

        // ─────────────────────────────────────────────
        // GetNode – bracket index notation
        // ─────────────────────────────────────────────

        [Fact]
        public void GetNode_BracketIndex_ReturnsCorrectItem()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "items", new List<object> { "first", "second" } }
            });
            Assert.Equal("second", node.GetNode("items[1]").AsString());
        }

        [Fact]
        public void GetNode_OutOfRangeIndex_ReturnsNullScalar()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "items", new List<object> { "only" } }
            });
            Assert.False(node.GetNode("items[5]").HasValue);
        }

        [Fact]
        public void GetNode_BracketQuotedKey_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "metrics", new Dictionary<string, object> { { "error.rate", "0.01" } } }
            });
            Assert.Equal("0.01", node.GetNode("metrics['error.rate']").AsString());
        }

        [Fact]
        public void GetNode_BracketDoubleQuotedKey_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "metrics", new Dictionary<string, object> { { "error.rate", "0.01" } } }
            });
            Assert.Equal("0.01", node.GetNode("metrics[\"error.rate\"]").AsString());
        }

        [Fact]
        public void GetNode_MixedPath_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                {
                    "servers", new List<object>
                    {
                        new Dictionary<string, object> { { "host", "localhost" } }
                    }
                }
            });
            Assert.Equal("localhost", node.GetNode("servers[0].host").AsString());
        }

        [Fact]
        public void GetNode_UnclosedBracket_ThrowsArgumentException()
        {
            var node = new DataNode(new Dictionary<string, object> { { "x", "y" } });
            Assert.Throws<ArgumentException>(() => node.GetNode("x[0"));
        }

        [Fact]
        public void GetNode_OnNonMap_ThrowsInvalidOperationException()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "scalar", "value" }
            });
            Assert.Throws<InvalidOperationException>(() => node.GetNode("scalar.nested"));
        }

        [Fact]
        public void GetNode_IndexOnNonSequence_ThrowsInvalidOperationException()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "scalar", "value" }
            });
            Assert.Throws<InvalidOperationException>(() => node.GetNode("scalar[0]"));
        }

        // ─────────────────────────────────────────────
        // Exists
        // ─────────────────────────────────────────────

        [Fact]
        public void Exists_ExistingKey_ReturnsTrue()
        {
            var node = new DataNode(new Dictionary<string, object> { { "key", "val" } });
            Assert.True(node.Exists("key"));
        }

        [Fact]
        public void Exists_MissingKey_ReturnsFalse()
        {
            var node = new DataNode(new Dictionary<string, object>());
            Assert.False(node.Exists("nope"));
        }

        [Fact]
        public void Exists_NullValue_ReturnsFalse()
        {
            var node = new DataNode(new Dictionary<string, object> { { "key", null } });
            Assert.False(node.Exists("key"));
        }

        [Fact]
        public void Exists_NestedPath_ReturnsTrue()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "a", new Dictionary<string, object> { { "b", "found" } } }
            });
            Assert.True(node.Exists("a.b"));
        }

        // ─────────────────────────────────────────────
        // GetSequenceNode
        // ─────────────────────────────────────────────

        [Fact]
        public void GetSequenceNode_ValidPath_ReturnsDataNodes()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "tags", new List<object> { "a", "b", "c" } }
            });
            var seq = node.GetSequenceNode("tags");
            Assert.Equal(3, seq.Count);
            Assert.Equal("a", seq[0].AsString());
        }

        [Fact]
        public void GetSequenceNode_NonSequencePath_ReturnsEmptyList()
        {
            var node = new DataNode(new Dictionary<string, object> { { "x", "scalar" } });
            Assert.Empty(node.GetSequenceNode("x"));
        }

        [Fact]
        public void GetSequenceNode_MissingPath_ReturnsEmptyList()
        {
            var node = new DataNode(new Dictionary<string, object>());
            Assert.Empty(node.GetSequenceNode("missing"));
        }

        // ─────────────────────────────────────────────
        // Indexer setter
        // ─────────────────────────────────────────────

        [Fact]
        public void Indexer_SetSimpleKey_StoresValue()
        {
            var node = new DataNode();
            node["name"] = "Alex";
            Assert.Equal("Alex", node.GetNode("name").AsString());
        }

        [Fact]
        public void Indexer_SetNestedKey_CreatesIntermediates()
        {
            var node = new DataNode();
            node["a.b"] = "deep";
            Assert.Equal("deep", node.GetNode("a.b").AsString());
        }

        [Fact]
        public void Indexer_OnScalarNode_ThrowsInvalidOperationException()
        {
            var node = new DataNode("scalar");
            Assert.Throws<InvalidOperationException>(() => node["key"] = "value");
        }

        // ─────────────────────────────────────────────
        // AsMap / AsSequence
        // ─────────────────────────────────────────────

        [Fact]
        public void AsMap_OnMapNode_ReturnsDictionary()
        {
            var node = new DataNode(new Dictionary<string, object> { { "k", "v" } });
            Assert.IsType<Dictionary<string, object>>(node.AsMap());
        }

        [Fact]
        public void AsMap_OnNonMap_ThrowsInvalidOperationException()
        {
            var node = new DataNode("text");
            Assert.Throws<InvalidOperationException>(() => node.AsMap());
        }

        [Fact]
        public void AsSequence_OnSequenceNode_ReturnsList()
        {
            var node = new DataNode(new List<object> { 1, 2 });
            Assert.IsType<List<object>>(node.AsSequence());
        }

        [Fact]
        public void AsSequence_OnNonSequence_ThrowsInvalidOperationException()
        {
            var node = new DataNode("text");
            Assert.Throws<InvalidOperationException>(() => node.AsSequence());
        }

        // ─────────────────────────────────────────────
        // Typed accessors – AsString
        // ─────────────────────────────────────────────

        [Fact]
        public void AsString_ReturnsStringValue()
        {
            Assert.Equal("hello", new DataNode("hello").AsString());
        }

        [Fact]
        public void AsString_OnNull_ReturnsNull()
        {
            Assert.Null(new DataNode((object)null).AsString());
        }

        [Fact]
        public void AsString_WithKeyPath_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object> { { "k", "v" } });
            Assert.Equal("v", node.AsString("k"));
        }

        [Fact]
        public void AsStringOrDefault_OnNull_ReturnsDefault()
        {
            Assert.Equal("default", new DataNode((object)null).AsStringOrDefault("default"));
        }

        [Fact]
        public void AsStringOrDefault_WithKeyPath_MissingReturnsDefault()
        {
            var node = new DataNode(new Dictionary<string, object>());
            Assert.Equal("fallback", node.AsStringOrDefault("missing", "fallback"));
        }

        // ─────────────────────────────────────────────
        // Typed accessors – AsInt
        // ─────────────────────────────────────────────

        [Fact]
        public void AsInt_StringNumber_Converts()
        {
            Assert.Equal(7, new DataNode("7").AsInt());
        }

        [Fact]
        public void AsInt_IntValue_Returns()
        {
            Assert.Equal(42, new DataNode(42).AsInt());
        }

        [Fact]
        public void AsIntOrDefault_OnNull_ReturnsDefault()
        {
            Assert.Equal(-1, new DataNode((object)null).AsIntOrDefault(-1));
        }

        [Fact]
        public void AsIntOrDefault_InvalidValue_ReturnsDefault()
        {
            Assert.Equal(0, new DataNode("not-a-number").AsIntOrDefault(0));
        }

        [Fact]
        public void AsInt_WithKeyPath_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object> { { "count", "10" } });
            Assert.Equal(10, node.AsInt("count"));
        }

        // ─────────────────────────────────────────────
        // Typed accessors – AsLong
        // ─────────────────────────────────────────────

        [Fact]
        public void AsLong_LargeValue_Converts()
        {
            Assert.Equal(9999999999L, new DataNode("9999999999").AsLong());
        }

        [Fact]
        public void AsLongOrDefault_InvalidValue_ReturnsDefault()
        {
            Assert.Equal(0L, new DataNode("bad").AsLongOrDefault(0L));
        }

        // ─────────────────────────────────────────────
        // Typed accessors – AsDouble
        // ─────────────────────────────────────────────

        [Fact]
        public void AsDouble_StringDecimal_Converts()
        {
            Assert.Equal(3.14, new DataNode("3.14").AsDouble(), precision: 5);
        }

        [Fact]
        public void AsDoubleOrDefault_InvalidValue_ReturnsDefault()
        {
            Assert.Equal(0.0, new DataNode("oops").AsDoubleOrDefault(0.0));
        }

        // ─────────────────────────────────────────────
        // Typed accessors – AsDecimal
        // ─────────────────────────────────────────────

        [Fact]
        public void AsDecimal_StringDecimal_Converts()
        {
            Assert.Equal(1.23m, new DataNode("1.23").AsDecimal());
        }

        [Fact]
        public void AsDecimalOrDefault_InvalidValue_ReturnsDefault()
        {
            Assert.Equal(0m, new DataNode("bad").AsDecimalOrDefault(0m));
        }

        // ─────────────────────────────────────────────
        // Typed accessors – AsBool
        // ─────────────────────────────────────────────

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        [InlineData("TRUE", true)]
        [InlineData("FALSE", false)]
        public void AsBool_StringVariants_Converts(string input, bool expected)
        {
            Assert.Equal(expected, new DataNode(input).AsBool());
        }

        [Fact]
        public void AsBool_BoolTrue_ReturnsTrue()
        {
            Assert.True(new DataNode(true).AsBool());
        }

        [Fact]
        public void AsBool_BoolFalse_ReturnsFalse()
        {
            Assert.False(new DataNode(false).AsBool());
        }

        [Fact]
        public void AsBool_InvalidString_ThrowsFormatException()
        {
            Assert.Throws<FormatException>(() => new DataNode("yes").AsBool());
        }

        [Fact]
        public void AsBoolOrDefault_InvalidValue_ReturnsDefault()
        {
            Assert.True(new DataNode("maybe").AsBoolOrDefault(true));
        }

        [Fact]
        public void AsBool_WithKeyPath_ReturnsCorrectValue()
        {
            var node = new DataNode(new Dictionary<string, object> { { "enabled", "true" } });
            Assert.True(node.AsBool("enabled"));
        }

        // ─────────────────────────────────────────────
        // Typed accessors – AsDateTime
        // ─────────────────────────────────────────────

        [Fact]
        public void AsDateTime_IsoString_Converts()
        {
            var node = new DataNode("2024-01-15");
            var dt = node.AsDateTime();
            Assert.Equal(2024, dt.Year);
            Assert.Equal(1, dt.Month);
            Assert.Equal(15, dt.Day);
        }

        [Fact]
        public void AsDateTimeOrDefault_InvalidValue_ReturnsDefault()
        {
            var fallback = new DateTime(2000, 1, 1);
            Assert.Equal(fallback, new DataNode("not-a-date").AsDateTimeOrDefault(fallback));
        }

        // ─────────────────────────────────────────────
        // ToString
        // ─────────────────────────────────────────────

        [Fact]
        public void ToString_Scalar_ReturnsValue()
        {
            Assert.Equal("hi", new DataNode("hi").ToString());
        }

        [Fact]
        public void ToString_NullScalar_ReturnsNullLabel()
        {
            Assert.Equal("(null)", new DataNode((object)null).ToString());
        }

        [Fact]
        public void ToString_Map_IncludesKeyCount()
        {
            var node = new DataNode(new Dictionary<string, object> { { "a", 1 }, { "b", 2 } });
            Assert.Contains("2 keys", node.ToString());
        }

        [Fact]
        public void ToString_Sequence_IncludesItemCount()
        {
            var node = new DataNode(new List<object> { "x", "y", "z" });
            Assert.Contains("3 items", node.ToString());
        }

        // ─────────────────────────────────────────────
        // Edge cases
        // ─────────────────────────────────────────────

        [Fact]
        public void GetNode_EmptyPath_ReturnsRootNode()
        {
            var node = new DataNode(new Dictionary<string, object> { { "k", "v" } });
            var result = node.GetNode("");
            Assert.Equal(NodeType.Map, result.Type);
        }

        [Fact]
        public void GetNode_NegativeIndex_ReturnsNull()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                { "items", new List<object> { "a" } }
            });
            Assert.False(node.GetNode("items[-1]").HasValue);
        }

        [Fact]
        public void Normalize_DeeplyNestedStructure_IsFullyNormalized()
        {
            var deep = new Dictionary<string, object>
            {
                {
                    "level1", new Dictionary<string, object>
                    {
                        { "level2", new List<object> { new Dictionary<string, object> { { "leaf", "value" } } } }
                    }
                }
            };
            var node = new DataNode(deep);
            Assert.Equal("value", node.GetNode("level1.level2[0].leaf").AsString());
        }
    }
}
