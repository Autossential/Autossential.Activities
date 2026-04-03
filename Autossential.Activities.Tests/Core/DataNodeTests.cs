using System.Globalization;
using System.Text.RegularExpressions;
using Autossential.Activities.Models;
using Xunit;

namespace Autossential.Activities.Tests.Core
{
    public class DataNodeTests
    {
        // ─── Construction & NodeType ─────────────────────────────────────────────

        [Theory]
        [InlineData(null)]
        [InlineData("hello")]
        [InlineData(42)]
        [InlineData(true)]
        [InlineData(3.14)]
        public void Constructor_ScalarValues_YieldScalarType(object value)
        {
            var node = new DataNode(value);
            Assert.Equal(NodeType.Scalar, node.Type);
        }

        [Fact]
        public void Constructor_Dictionary_YieldsMapType()
        {
            var node = new DataNode(new Dictionary<string, object> { ["k"] = 1 });
            Assert.Equal(NodeType.Map, node.Type);
        }

        [Fact]
        public void Constructor_List_YieldsSequenceType()
        {
            var node = new DataNode(new List<object> { 1, 2 });
            Assert.Equal(NodeType.Sequence, node.Type);
        }

        [Fact]
        public void Constructor_DataNodeValue_UnwrapsRawValue()
        {
            var inner = new DataNode("unwrapped");
            var node = new DataNode(inner);
            Assert.Equal("unwrapped", node.RawValue);
        }

        [Fact]
        public void DefaultConstructor_CreatesEmptyMap()
        {
            var node = new DataNode();
            Assert.Equal(NodeType.Map, node.Type);
            Assert.Empty(node.Keys);
        }

        [Fact]
        public void Empty_CreatesEmptyMapWithCulture()
        {
            var culture = CultureInfo.GetCultureInfo("pt-BR");
            var node = DataNode.Empty(culture);
            Assert.Equal(NodeType.Map, node.Type);
            Assert.Equal(culture, node.Culture);
        }

        [Fact]
        public void Constructor_NullCulture_FallsBackToInvariant()
        {
            var node = new DataNode("x", null);
            Assert.Equal(CultureInfo.InvariantCulture, node.Culture);
        }

        // ─── Normalize: IDictionary / IEnumerable ────────────────────────────────

        [Fact]
        public void Normalize_Hashtable_ConvertsToDictionaryStringObject()
        {
            var ht = new System.Collections.Hashtable { ["key"] = 99 };
            var node = new DataNode(ht);
            Assert.Equal(NodeType.Map, node.Type);
            Assert.Equal(99, node["key"].AsInt());
        }

        [Fact]
        public void Normalize_Array_ConvertsToSequence()
        {
            var node = new DataNode(new[] { "a", "b", "c" });
            Assert.Equal(NodeType.Sequence, node.Type);
            Assert.Equal(3, node.AsSequence<string>().Count);
        }

        // ─── HasValue / Exists ───────────────────────────────────────────────────

        [Theory]
        [InlineData("value", true)]
        [InlineData(null, false)]
        public void HasValue_ReflectsNullness(object value, bool expected)
        {
            var node = new DataNode(value);
            Assert.Equal(expected, node.HasValue());
        }

        [Fact]
        public void Exists_ExistingKey_ReturnsTrue()
        {
            var node = new DataNode(new Dictionary<string, object> { ["a"] = 1 });
            Assert.True(node.Exists("a"));
        }

        [Fact]
        public void Exists_MissingKey_ReturnsFalse()
        {
            var node = new DataNode(new Dictionary<string, object> { ["a"] = 1 });
            Assert.False(node.Exists("z"));
        }

        [Fact]
        public void HasValue_KeyPath_FalseForNullEntry()
        {
            var node = new DataNode(new Dictionary<string, object> { ["x"] = null });
            Assert.False(node.HasValue("x"));
        }

        // ─── Path Navigation ─────────────────────────────────────────────────────

        [Fact]
        public void GetNode_DotSeparatedPath_ReturnsCorrectNode()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["a"] = new Dictionary<string, object> { ["b"] = "deep" }
            });
            Assert.Equal("deep", node.GetNode("a.b").AsString());
        }

        [Fact]
        public void GetNode_IndexPath_ReturnsCorrectElement()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["items"] = new List<object> { "x", "y", "z" }
            });
            Assert.Equal("y", node.GetNode("items[1]").AsString());
        }

        [Fact]
        public void GetNode_QuotedKey_NavigatesDottedKey()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["error.rate"] = 0.5
            });
            Assert.Equal(0.5, node.GetNode("['error.rate']").AsDouble());
        }

        [Fact]
        public void GetNode_MissingPath_ReturnsNullScalar()
        {
            var node = new DataNode(new Dictionary<string, object> { ["a"] = 1 });
            var result = node.GetNode("missing");
            Assert.Equal(NodeType.Scalar, result.Type);
            Assert.Null(result.RawValue);
        }

        [Fact]
        public void GetNode_OutOfBoundsIndex_ReturnsNullScalar()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["list"] = new List<object> { 1 }
            });
            var result = node.GetNode("list[99]");
            Assert.Null(result.RawValue);
        }

        [Theory]
        [InlineData("server.host")]
        [InlineData("SERVER.Host")]
        [InlineData("server.Security.ROLES.admin")]
        public void GetNode_InsensitiveCase_ReturnsValue(string keyPath)
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["server"] = new Dictionary<string, object>
                {
                    ["host"] = "api.example.com",
                    ["port"] = 443,
                    ["protocols"] = new string[] { "https", "http2" },
                    ["features"] = new List<string> { "gzip", "caching", "rate-limiting" },
                    ["security"] = new Dictionary<string, object>
                    {
                        ["enabled"] = true,
                        ["allowedIPs"] = new string[] { "192.168.1.10", "10.0.0.5" },
                        ["roles"] = new Dictionary<string, object>
                        {
                            ["admin"] = new List<string> { "create", "delete", "update", "read" },
                            ["user"] = new List<string> { "read", "update" },
                            ["guest"] = new string[] { "read" }
                        }
                    }
                }
            });

            var result = node.GetNode(keyPath);
            Assert.NotNull(result);
        }

        [Fact]
        public void Indexer_Set_WritesValueIntoMap()
        {
            var node = new DataNode();
            node["greeting"] = new DataNode("hello");
            Assert.Equal("hello", node["greeting"].AsString());
        }

        [Fact]
        public void Indexer_Set_CreatesIntermediateMaps()
        {
            var node = new DataNode();
            node["a.b"] = new DataNode(42);
            Assert.Equal(42, node["a.b"].AsInt());
        }

        // ─── Keys ────────────────────────────────────────────────────────────────

        [Fact]
        public void Keys_MapNode_ReturnsAllKeys()
        {
            var node = new DataNode(new Dictionary<string, object> { ["x"] = 1, ["y"] = 2 });
            Assert.Equal(new[] { "x", "y" }, node.Keys.OrderBy(k => k));
        }

        [Fact]
        public void Keys_NonMapNode_ReturnsEmpty()
        {
            var node = new DataNode("scalar");
            Assert.Empty(node.Keys);
        }

        // ─── Scalar Conversions ──────────────────────────────────────────────────

        [Theory]
        [InlineData("42", 42)]
        [InlineData(42, 42)]
        [InlineData(42.3, 42)]
        [InlineData(42.9, 43)]
        public void AsInt_VariousInputs_Converts(object value, int expected)
        {
            Assert.Equal(expected, new DataNode(value).AsInt());
        }

        [Theory]
        [InlineData("3.14")]
        [InlineData("not-a-number")]
        public void AsInt_InvalidInput_ThrowsOrReturnsDefault(string value)
        {
            var node = new DataNode(value);
            Assert.Throws<FormatException>(() => node.AsInt());
            Assert.Equal(-1, node.AsIntOrDefault(-1));
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("1", true)]
        [InlineData("false", false)]
        [InlineData("0", false)]
        [InlineData(true, true)]
        public void AsBool_StringAndBoolInputs_Converts(object value, bool expected)
        {
            Assert.Equal(expected, new DataNode(value).AsBool());
        }

        [Fact]
        public void AsBool_InvalidString_Throws()
        {
            Assert.Throws<FormatException>(() => new DataNode("yes").AsBool());
        }

        [Theory]
        [InlineData("1.5", 1.5f)]
        [InlineData(1.5f, 1.5f)]
        public void AsFloat_Converts(object value, float expected)
        {
            Assert.Equal(expected, new DataNode(value).AsFloat(), precision: 5);
        }

        [Theory]
        [InlineData("1.5", 1.5)]
        [InlineData(1.5, 1.5)]
        public void AsDouble_Converts(object value, double expected)
        {
            Assert.Equal(expected, new DataNode(value).AsDouble(), precision: 10);
        }

        [Theory]
        [InlineData("3.14")]
        [InlineData("0")]
        public void AsDecimal_ValidString_Converts(string value)
        {
            var expected = decimal.Parse(value, CultureInfo.InvariantCulture);
            Assert.Equal(expected, new DataNode(value).AsDecimal());
        }

        [Fact]
        public void AsString_NullValue_ReturnsNull()
        {
            Assert.Null(new DataNode(null).AsString());
        }

        [Fact]
        public void AsStringOrDefault_NullValue_ReturnsDefault()
        {
            Assert.Equal("default", new DataNode(null).AsStringOrDefault("default"));
        }

        [Fact]
        public void AsDateTime_ValidString_Converts()
        {
            var dt = new DateTime(2024, 6, 1);
            var node = new DataNode(dt.ToString(CultureInfo.InvariantCulture));
            Assert.Equal(dt.Date, node.AsDateTime().Date);
        }

        [Fact]
        public void AsDateTimeOrDefault_Invalid_ReturnsDefault()
        {
            var fallback = new DateTime(2000, 1, 1);
            Assert.Equal(fallback, new DataNode("not-a-date").AsDateTimeOrDefault(fallback));
        }

        [Fact]
        public void AsRegex_ValidPattern_ReturnsRegex()
        {
            var node = new DataNode(@"\d+");
            var regex = node.AsRegex();
            Assert.Matches(regex, "123");
        }

        [Fact]
        public void AsRegex_WithOptions_AppliesOptions()
        {
            var node = new DataNode("hello");
            var regex = node.AsRegex(RegexOptions.IgnoreCase);
            Assert.Matches(regex, "HELLO");
        }

        [Fact]
        public void AsRegexOrDefault_NullValue_ReturnsDefault()
        {
            var fallback = new Regex("fallback");
            var result = new DataNode(null).AsRegexOrDefault(fallback);
            Assert.Same(fallback, result);
        }

        // ─── OrDefault shortcuts (null node) ────────────────────────────────────

        [Fact]
        public void OrDefault_NullNode_ReturnsDefault()
        {
            var node = new DataNode(null);
            Assert.Equal(99, node.AsIntOrDefault(99));
            Assert.Equal(99L, node.AsLongOrDefault(99));
            Assert.Equal(9.9f, node.AsFloatOrDefault(9.9f));
            Assert.Equal(9.9, node.AsDoubleOrDefault(9.9));
            Assert.Equal(9.9m, node.AsDecimalOrDefault(9.9m));
            Assert.True(node.AsBoolOrDefault(true));
        }

        // ─── AsSequence ──────────────────────────────────────────────────────────

        [Fact]
        public void AsSequence_IntList_ConvertsElements()
        {
            var node = new DataNode(new List<object> { 1, 2, 3 });
            var result = node.AsSequence<int>();
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void AsSequence_NonSequenceNode_Throws()
        {
            var node = new DataNode("scalar");
            Assert.Throws<InvalidOperationException>(() => node.AsSequence<string>());
        }

        [Fact]
        public void AsSequenceOrDefault_NonSequenceNode_ReturnsDefault()
        {
            var fallback = new List<string> { "default" };
            var result = new DataNode("scalar").AsSequenceOrDefault(fallback);
            Assert.Same(fallback, result);
        }

        [Fact]
        public void AsSequence_DataNode_ConvertsElements()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["NotesArray"] = new string[] { "Fourth level", "Arrays included", "Mixed types" }
            });

            var result = node.AsSequence<DataNode>("NotesArray");
            Assert.Equal(3, result.Count);
        }

        // ─── AsMap ───────────────────────────────────────────────────────────────

        [Fact]
        public void AsMap_MapNode_ReturnsDictionary()
        {
            var node = new DataNode(new Dictionary<string, object> { ["k"] = "v" });
            var map = node.AsMap();
            Assert.Equal("v", map["k"]);
        }

        [Fact]
        public void AsMap_NonMapNode_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => new DataNode("x").AsMap());
        }

        // ─── Merge ───────────────────────────────────────────────────────────────

        [Fact]
        public void Merge_NewKey_AddsToTarget()
        {
            var target = new DataNode(new Dictionary<string, object> { ["a"] = 1 });
            var source = new DataNode(new Dictionary<string, object> { ["b"] = 2 });
            target.Merge(source);
            Assert.Equal(2, target["b"].AsInt());
        }

        [Fact]
        public void Merge_OverlappingScalar_SourceOverwrites()
        {
            var target = new DataNode(new Dictionary<string, object> { ["x"] = "old" });
            var source = new DataNode(new Dictionary<string, object> { ["x"] = "new" });
            target.Merge(source);
            Assert.Equal("new", target["x"].AsString());
        }

        [Fact]
        public void Merge_NestedMap_MergesRecursively()
        {
            var target = new DataNode(new Dictionary<string, object>
            {
                ["outer"] = new Dictionary<string, object> { ["a"] = 1 }
            });
            var source = new DataNode(new Dictionary<string, object>
            {
                ["outer"] = new Dictionary<string, object> { ["b"] = 2 }
            });
            target.Merge(source);
            Assert.Equal(1, target["outer.a"].AsInt());
            Assert.Equal(2, target["outer.b"].AsInt());
        }

        [Fact]
        public void Merge_NullSource_LeavesTargetUnchanged()
        {
            var target = new DataNode(new Dictionary<string, object> { ["a"] = 1 });
            target.Merge(null);
            Assert.Equal(1, target["a"].AsInt());
        }

        [Fact]
        public void Merge_NonMapTarget_Throws()
        {
            var node = new DataNode("scalar");
            Assert.Throws<InvalidOperationException>(() =>
                node.Merge(new DataNode(new Dictionary<string, object>())));
        }

        // ─── ToString ────────────────────────────────────────────────────────────

        [Theory]
        [InlineData("hello", "hello")]
        [InlineData(null, "(null)")]
        public void ToString_Scalar_ReturnsExpected(object value, string expected)
        {
            Assert.Equal(expected, new DataNode(value).ToString());
        }

        [Fact]
        public void ToString_Sequence_ReturnsItemCount()
        {
            var node = new DataNode(new List<object> { 1, 2 });
            Assert.Equal("[Sequence, 2 items]", node.ToString());
        }

        [Fact]
        public void ToString_Map_ReturnsKeyCount()
        {
            var node = new DataNode(new Dictionary<string, object> { ["a"] = 1, ["b"] = 2 });
            Assert.Equal("[Map, 2 keys]", node.ToString());
        }

        // ─── Navigation Methods (partial class) ──────────────────────────────────

        [Fact]
        public void NavigationMethods_AsInt_ReturnsValueAtPath()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["count"] = 7
            });
            Assert.Equal(7, node.AsInt("count"));
            Assert.Equal(0, node.AsIntOrDefault("missing", 0));
        }

        [Fact]
        public void NavigationMethods_AsSequence_ReturnsListAtPath()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["tags"] = new List<object> { "a", "b" }
            });
            Assert.Equal(new[] { "a", "b" }, node.AsSequence<string>("tags"));
        }

        [Fact]
        public void NavigationMethods_AsRegex_ReturnsRegexAtPath()
        {
            var node = new DataNode(new Dictionary<string, object>
            {
                ["pattern"] = @"\w+"
            });
            var regex = node.AsRegex("pattern");
            Assert.Matches(regex, "word");
        }

        [Fact]
        public void NavigationMethods_AsRegexOrDefault_MissingPath_ReturnsDefault()
        {
            var fallback = new Regex("fallback");
            var node = new DataNode(new Dictionary<string, object>());
            var result = node.AsRegexOrDefault("missing", fallback);
            Assert.Same(fallback, result);
        }
    }
}