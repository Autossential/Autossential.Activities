using Autossential.Activities.Core;
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
    // ─────────────────────────────────────────────────────────────────────────
    //  DataNodeTests
    //
    //  Coverage:
    //    1.  Construction & Normalization
    //    2.  NodeType detection
    //    3.  RawValue & HasValue
    //    4.  Keys
    //    5.  Exists
    //    6.  GetNode
    //    7.  GetSequenceNode
    //    8.  Indexer (get & set)
    //    9.  AsMap / AsSequence
    //    10. AsString / AsStringOrDefault
    //    11. AsInt / AsIntOrDefault
    //    12. AsLong / AsLongOrDefault
    //    13. AsDouble / AsDoubleOrDefault
    //    14. AsDecimal / AsDecimalOrDefault
    //    15. AsDateTime / AsDateTimeOrDefault
    //    16. AsBool / AsBoolOrDefault
    //    17. Merge
    //    18. ToString
    //    19. CultureInfo
    // ─────────────────────────────────────────────────────────────────────────
    public class DataNodeTests
    {
        private static DataNode SampleMap() => new(new Dictionary<string, object>
        {
            ["name"] = "Alice",
            ["age"] = "30",
            ["active"] = "true",
            ["score"] = "9.5",
            ["tags"] = new List<object> { "admin", "user" },
            ["address"] = new Dictionary<string, object>
            {
                ["city"] = "Curitiba",
                ["state"] = "PR",
                ["zipcode"] = null
            }
        });

        // ═════════════════════════════════════════════════════════════════════
        //  1. Construction & Normalization
        // ═════════════════════════════════════════════════════════════════════
        public class ConstructionAndNormalization
        {
            [Fact]
            public void NullValue_CreatesScalar()
            {
                var node = new DataNode(null);
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.Null(node.RawValue);
            }

            [Fact]
            public void StringValue_CreatesScalar()
            {
                var node = new DataNode("hello");
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.Equal("hello", node.RawValue);
            }

            [Fact]
            public void IntValue_CreatesScalar()
            {
                var node = new DataNode(42);
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.Equal(42, node.RawValue);
            }

            [Fact]
            public void BoolValue_CreatesScalar()
            {
                var node = new DataNode(true);
                Assert.Equal(NodeType.Scalar, node.Type);
            }

            [Fact]
            public void DateTimeValue_CreatesScalar()
            {
                var dt = new DateTime(2024, 1, 15);
                var node = new DataNode(dt);
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.Equal(dt, node.RawValue);
            }

            [Fact]
            public void GuidValue_CreatesScalar()
            {
                var guid = Guid.NewGuid();
                var node = new DataNode(guid);
                Assert.Equal(NodeType.Scalar, node.Type);
            }

            [Fact]
            public void DictionaryStringObject_CreatesMap()
            {
                var node = new DataNode(new Dictionary<string, object> { ["k"] = "v" });
                Assert.Equal(NodeType.Map, node.Type);
            }

            [Fact]
            public void DictionaryStringString_NormalizesToMap()
            {
                var node = new DataNode(new Dictionary<string, string> { ["k"] = "v" });
                Assert.Equal(NodeType.Map, node.Type);
                Assert.IsType<Dictionary<string, object>>(node.RawValue);
            }

            [Fact]
            public void DictionaryIntObject_NormalizesKeysToString()
            {
                var node = new DataNode(new Dictionary<int, object> { [1] = "one" });
                Assert.Equal(NodeType.Map, node.Type);
                var map = (Dictionary<string, object>)node.RawValue;
                Assert.True(map.ContainsKey("1"));
            }

            [Fact]
            public void ListObject_CreatesSequence()
            {
                var node = new DataNode(new List<object> { "a", "b" });
                Assert.Equal(NodeType.Sequence, node.Type);
            }

            [Fact]
            public void ListString_NormalizesToSequence()
            {
                var node = new DataNode(new List<string> { "a", "b" });
                Assert.Equal(NodeType.Sequence, node.Type);
                Assert.IsType<List<object>>(node.RawValue);
            }

            [Fact]
            public void Array_NormalizesToSequence()
            {
                var node = new DataNode(new[] { 1, 2, 3 });
                Assert.Equal(NodeType.Sequence, node.Type);
                Assert.IsType<List<object>>(node.RawValue);
            }

            [Fact]
            public void NestedDataNode_IsUnwrapped()
            {
                var inner = new DataNode("value");
                var outer = new DataNode(inner);
                Assert.Equal(NodeType.Scalar, outer.Type);
                Assert.Equal("value", outer.RawValue);
            }

            [Fact]
            public void NestedDictionary_IsNormalizedRecursively()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["nested"] = new Dictionary<string, string> { ["k"] = "v" }
                });
                var map = (Dictionary<string, object>)node.RawValue;
                var nested = map["nested"];
                Assert.IsType<Dictionary<string, object>>(nested);
            }

            [Fact]
            public void NestedList_IsNormalizedRecursively()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["items"] = new List<string> { "a", "b" }
                });
                var map = (Dictionary<string, object>)node.RawValue;
                var items = map["items"];
                Assert.IsType<List<object>>(items);
            }

            [Fact]
            public void ParameterlessConstructor_CreatesEmptyMap()
            {
                var node = new DataNode();
                Assert.Equal(NodeType.Map, node.Type);
                Assert.Empty(((Dictionary<string, object>)node.RawValue));
            }

            [Fact]
            public void CultureInfo_DefaultsToInvariant()
            {
                var node = new DataNode("x");
                Assert.Equal(CultureInfo.InvariantCulture, node.Culture);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  2. NodeType detection
        // ═════════════════════════════════════════════════════════════════════
        public class NodeTypeDetection
        {
            [Fact]
            public void Null_IsScalar() => Assert.Equal(NodeType.Scalar, new DataNode(null).Type);

            [Fact]
            public void String_IsScalar() => Assert.Equal(NodeType.Scalar, new DataNode("x").Type);

            [Fact]
            public void Int_IsScalar() => Assert.Equal(NodeType.Scalar, new DataNode(1).Type);

            [Fact]
            public void Dict_IsMap() => Assert.Equal(NodeType.Map, new DataNode(new Dictionary<string, object>()).Type);

            [Fact]
            public void List_IsSequence() => Assert.Equal(NodeType.Sequence, new DataNode(new List<object>()).Type);
        }

        // ═════════════════════════════════════════════════════════════════════
        //  3. RawValue & HasValue
        // ═════════════════════════════════════════════════════════════════════
        public class RawValueAndHasValue
        {
            [Fact]
            public void HasValue_NullRawValue_ReturnsFalse()
            {
                var node = new DataNode(null);
                Assert.False(node.HasValue());
            }

            [Fact]
            public void HasValue_NonNullRawValue_ReturnsTrue()
            {
                var node = new DataNode("hello");
                Assert.True(node.HasValue());
            }

            [Fact]
            public void HasValue_WithKeyPath_ExistsAndNonNull_ReturnsTrue()
            {
                var node = SampleMap();
                Assert.True(node.HasValue("name"));
            }

            [Fact]
            public void HasValue_WithKeyPath_ExistsButNull_ReturnsFalse()
            {
                var node = SampleMap();
                Assert.False(node.HasValue("address.zipcode"));
            }

            [Fact]
            public void HasValue_WithKeyPath_NotExists_ReturnsFalse()
            {
                var node = SampleMap();
                Assert.False(node.HasValue("nonexistent"));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  4. Keys
        // ═════════════════════════════════════════════════════════════════════
        public class KeysProperty
        {
            [Fact]
            public void Keys_OnMap_ReturnsAllKeys()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["a"] = 1,
                    ["b"] = 2,
                    ["c"] = 3
                });
                Assert.Equal(3, new List<string>(node.Keys).Count);
                Assert.Contains("a", node.Keys);
                Assert.Contains("b", node.Keys);
                Assert.Contains("c", node.Keys);
            }

            [Fact]
            public void Keys_OnScalar_ReturnsEmpty()
            {
                var node = new DataNode("scalar");
                Assert.Empty(node.Keys);
            }

            [Fact]
            public void Keys_OnSequence_ReturnsEmpty()
            {
                var node = new DataNode(new List<object> { "a", "b" });
                Assert.Empty(node.Keys);
            }

            [Fact]
            public void Keys_OnEmptyMap_ReturnsEmpty()
            {
                var node = new DataNode();
                Assert.Empty(node.Keys);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  5. Exists
        // ═════════════════════════════════════════════════════════════════════
        public class ExistsMethod
        {
            [Fact]
            public void Exists_ExistingKey_ReturnsTrue()
            {
                var node = SampleMap();
                Assert.True(node.Exists("name"));
            }

            [Fact]
            public void Exists_NonExistingKey_ReturnsFalse()
            {
                var node = SampleMap();
                Assert.False(node.Exists("nonexistent"));
            }

            [Fact]
            public void Exists_NestedKey_ReturnsTrue()
            {
                var node = SampleMap();
                Assert.True(node.Exists("address.city"));
            }

            [Fact]
            public void Exists_NullValueKey_ReturnsTrue()
            {
                var node = SampleMap();
                Assert.True(node.Exists("address.zipcode"));
            }

            [Fact]
            public void Exists_SequenceIndex_ReturnsTrue()
            {
                var node = SampleMap();
                Assert.True(node.Exists("tags[0]"));
            }

            [Fact]
            public void Exists_OutOfRangeIndex_ReturnsFalse()
            {
                var node = SampleMap();
                Assert.False(node.Exists("tags[99]"));
            }

            [Fact]
            public void Exists_InvalidStructuralPath_ReturnsFalse()
            {
                var node = SampleMap();
                Assert.False(node.Exists("tags[0].invalid"));
            }

            [Fact]
            public void Exists_EmptyPath_ReturnsFalse()
            {
                var node = SampleMap();
                Assert.False(node.Exists(""));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  6. GetNode
        // ═════════════════════════════════════════════════════════════════════
        public class GetNodeMethod
        {
            [Fact]
            public void GetNode_ExistingKey_ReturnsCorrectNode()
            {
                var node = SampleMap();
                var result = node.GetNode("name");
                Assert.Equal("Alice", result.AsString());
            }

            [Fact]
            public void GetNode_NestedKey_ReturnsCorrectNode()
            {
                var node = SampleMap();
                var result = node.GetNode("address.city");
                Assert.Equal("Curitiba", result.AsString());
            }

            [Fact]
            public void GetNode_SequenceIndex_ReturnsCorrectNode()
            {
                var node = SampleMap();
                var result = node.GetNode("tags[0]");
                Assert.Equal("admin", result.AsString());
            }

            [Fact]
            public void GetNode_NonExistingKey_ReturnsNullScalar()
            {
                var node = SampleMap();
                var result = node.GetNode("nonexistent");
                Assert.Equal(NodeType.Scalar, result.Type);
                Assert.Null(result.RawValue);
            }

            [Fact]
            public void GetNode_NullValueKey_ReturnsNullScalar()
            {
                var node = SampleMap();
                var result = node.GetNode("address.zipcode");
                Assert.False(result.HasValue());
            }

            [Fact]
            public void GetNode_InvalidIndex_ThrowsInvalidOperation()
            {
                var node = SampleMap();
                Assert.Throws<InvalidOperationException>(() => node.GetNode("name[0]"));
            }

            [Fact]
            public void GetNode_QuotedKeyWithDots_ReturnsCorrectNode()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["error.rate"] = "0.5"
                });
                var result = node.GetNode("['error.rate']");
                Assert.Equal("0.5", result.AsString());
            }

            [Fact]
            public void GetNode_ChainedIndexes_ReturnsCorrectNode()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["matrix"] = new List<object>
                    {
                        new List<object> { "a", "b" },
                        new List<object> { "c", "d" }
                    }
                });
                Assert.Equal("d", node.GetNode("matrix[1][1]").AsString());
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  7. GetSequenceNode
        // ═════════════════════════════════════════════════════════════════════
        public class GetSequenceNodeMethod
        {
            [Fact]
            public void GetSequenceNode_ExistingSequence_ReturnsDataNodeList()
            {
                var node = SampleMap();
                var items = node.GetSequenceNode("tags");
                Assert.Equal(2, items.Count);
                Assert.IsType<DataNode>(items[0]);
            }

            [Fact]
            public void GetSequenceNode_CorrectValues()
            {
                var node = SampleMap();
                var items = node.GetSequenceNode("tags");
                Assert.Equal("admin", items[0].AsString());
                Assert.Equal("user", items[1].AsString());
            }

            [Fact]
            public void GetSequenceNode_NonExistingPath_ReturnsEmpty()
            {
                var node = SampleMap();
                var items = node.GetSequenceNode("nonexistent");
                Assert.Empty(items);
            }

            [Fact]
            public void GetSequenceNode_NonSequencePath_ReturnsEmpty()
            {
                var node = SampleMap();
                var items = node.GetSequenceNode("name");
                Assert.Empty(items);
            }

            [Fact]
            public void GetSequenceNode_ItemsInheritCulture()
            {
                var culture = new CultureInfo("pt-BR");
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["items"] = new List<object> { "a", "b" }
                }, culture);
                var items = node.GetSequenceNode("items");
                Assert.Equal(culture, items[0].Culture);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  8. Indexer
        // ═════════════════════════════════════════════════════════════════════
        public class Indexer
        {
            [Fact]
            public void Getter_ReturnsCorrectNode()
            {
                var node = SampleMap();
                Assert.Equal("Alice", node["name"].AsString());
            }

            [Fact]
            public void Getter_NestedPath_ReturnsCorrectNode()
            {
                var node = SampleMap();
                Assert.Equal("Curitiba", node["address.city"].AsString());
            }

            [Fact]
            public void Getter_InvalidStructuralPath_Throws()
            {
                var node = SampleMap();
                Assert.Throws<InvalidOperationException>(() => node["tags[0].invalid"].AsString());
            }

            [Fact]
            public void Setter_AddsNewKey()
            {
                var node = new DataNode();
                node["newkey"] = new DataNode("value");
                Assert.Equal("value", node.AsString("newkey"));
            }

            [Fact]
            public void Setter_OverwritesExistingKey()
            {
                var node = SampleMap();
                node["name"] = new DataNode("Bob");
                Assert.Equal("Bob", node.AsString("name"));
            }

            [Fact]
            public void Setter_NestedPath_CreatesIntermediateMaps()
            {
                var node = new DataNode();
                node["a.b.c"] = new DataNode("deep");
                Assert.Equal("deep", node.AsString("a.b.c"));
            }

            [Fact]
            public void Setter_NestedPath_OverwritesExisting()
            {
                var node = SampleMap();
                node["address.city"] = new DataNode("São Paulo");
                Assert.Equal("São Paulo", node.AsString("address.city"));
            }

            [Fact]
            public void Setter_NullDataNode_StoresNull()
            {
                var node = new DataNode();
                node["key"] = new DataNode(null);
                Assert.True(node.Exists("key"));
                Assert.False(node.HasValue("key"));
            }

            [Fact]
            public void Setter_OnNonMap_Throws()
            {
                var node = new DataNode("scalar");
                Assert.Throws<InvalidOperationException>(() => node["key"] = new DataNode("v"));
            }

            [Fact]
            public void Setter_WithIndex_UpdatesSequenceItem()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["items"] = new List<object> { "a", "b", "c" }
                });
                node["items[1]"] = new DataNode("updated");
                Assert.Equal("updated", node.AsString("items[1]"));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  9. AsMap / AsSequence
        // ═════════════════════════════════════════════════════════════════════
        public class AsMapAndAsSequence
        {
            [Fact]
            public void AsMap_OnMap_ReturnsDictionary()
            {
                var node = SampleMap();
                Assert.IsType<Dictionary<string, object>>(node.AsMap());
            }

            [Fact]
            public void AsMap_OnNonMap_Throws()
            {
                var node = new DataNode("scalar");
                Assert.Throws<InvalidOperationException>(() => node.AsMap());
            }

            [Fact]
            public void AsSequence_OnSequence_ReturnsList()
            {
                var node = new DataNode(new List<object> { "a", "b" });
                Assert.IsType<List<object>>(node.AsSequence());
            }

            [Fact]
            public void AsSequence_OnNonSequence_Throws()
            {
                var node = new DataNode("scalar");
                Assert.Throws<InvalidOperationException>(() => node.AsSequence());
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  10. AsString / AsStringOrDefault
        // ═════════════════════════════════════════════════════════════════════
        public class AsStringMethods
        {
            [Fact]
            public void AsString_ReturnsValue()
            {
                Assert.Equal("Alice", new DataNode("Alice").AsString());
            }

            [Fact]
            public void AsString_NullValue_ReturnsNull()
            {
                Assert.Null(new DataNode(null).AsString());
            }

            [Fact]
            public void AsString_WithKeyPath_ReturnsValue()
            {
                Assert.Equal("Alice", SampleMap().AsString("name"));
            }

            [Fact]
            public void AsString_WithKeyPath_NonExisting_ReturnsNull()
            {
                Assert.Null(SampleMap().AsString("nonexistent"));
            }

            [Fact]
            public void AsStringOrDefault_NullValue_ReturnsDefault()
            {
                Assert.Equal("default", new DataNode(null).AsStringOrDefault("default"));
            }

            [Fact]
            public void AsStringOrDefault_NonNull_ReturnsValue()
            {
                Assert.Equal("Alice", new DataNode("Alice").AsStringOrDefault("default"));
            }

            [Fact]
            public void AsStringOrDefault_WithKeyPath_NonExisting_ReturnsDefault()
            {
                Assert.Equal("N/A", SampleMap().AsStringOrDefault("nonexistent", "N/A"));
            }

            [Fact]
            public void AsStringOrDefault_WithKeyPath_Existing_ReturnsValue()
            {
                Assert.Equal("Alice", SampleMap().AsStringOrDefault("name", "N/A"));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  11. AsInt / AsIntOrDefault
        // ═════════════════════════════════════════════════════════════════════
        public class AsIntMethods
        {
            [Fact]
            public void AsInt_ValidString_ReturnsInt()
            {
                Assert.Equal(30, new DataNode("30").AsInt());
            }

            [Fact]
            public void AsInt_IntValue_ReturnsInt()
            {
                Assert.Equal(42, new DataNode(42).AsInt());
            }

            [Fact]
            public void AsInt_WithKeyPath_ReturnsInt()
            {
                Assert.Equal(30, SampleMap().AsInt("age"));
            }

            [Fact]
            public void AsInt_InvalidString_Throws()
            {
                Assert.Throws<FormatException>(() => new DataNode("notanumber").AsInt());
            }

            [Fact]
            public void AsIntOrDefault_InvalidString_ReturnsDefault()
            {
                Assert.Equal(0, new DataNode("notanumber").AsIntOrDefault(0));
            }

            [Fact]
            public void AsIntOrDefault_NullValue_ReturnsDefault()
            {
                Assert.Equal(-1, new DataNode(null).AsIntOrDefault(-1));
            }

            [Fact]
            public void AsIntOrDefault_WithKeyPath_NonExisting_ReturnsDefault()
            {
                Assert.Equal(99, SampleMap().AsIntOrDefault("nonexistent", 99));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  12. AsLong / AsLongOrDefault
        // ═════════════════════════════════════════════════════════════════════
        public class AsLongMethods
        {
            [Fact]
            public void AsLong_ValidString_ReturnsLong()
            {
                Assert.Equal(9999999999L, new DataNode("9999999999").AsLong());
            }

            [Fact]
            public void AsLong_WithKeyPath_ReturnsLong()
            {
                Assert.Equal(30L, SampleMap().AsLong("age"));
            }

            [Fact]
            public void AsLongOrDefault_NullValue_ReturnsDefault()
            {
                Assert.Equal(0L, new DataNode(null).AsLongOrDefault(0L));
            }

            [Fact]
            public void AsLongOrDefault_InvalidString_ReturnsDefault()
            {
                Assert.Equal(-1L, new DataNode("invalid").AsLongOrDefault(-1L));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  13. AsDouble / AsDoubleOrDefault
        // ═════════════════════════════════════════════════════════════════════
        public class AsDoubleMethods
        {
            [Fact]
            public void AsDouble_ValidString_ReturnsDouble()
            {
                Assert.Equal(9.5, new DataNode("9.5").AsDouble());
            }

            [Fact]
            public void AsDouble_WithKeyPath_ReturnsDouble()
            {
                Assert.Equal(9.5, SampleMap().AsDouble("score"));
            }

            [Fact]
            public void AsDoubleOrDefault_NullValue_ReturnsDefault()
            {
                Assert.Equal(0.0, new DataNode(null).AsDoubleOrDefault(0.0));
            }

            [Fact]
            public void AsDoubleOrDefault_InvalidString_ReturnsDefault()
            {
                Assert.Equal(-1.0, new DataNode("invalid").AsDoubleOrDefault(-1.0));
            }

            [Fact]
            public void AsDoubleOrDefault_WithKeyPath_NonExisting_ReturnsDefault()
            {
                Assert.Equal(1.5, SampleMap().AsDoubleOrDefault("nonexistent", 1.5));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  14. AsDecimal / AsDecimalOrDefault
        // ═════════════════════════════════════════════════════════════════════
        public class AsDecimalMethods
        {
            [Fact]
            public void AsDecimal_ValidString_ReturnsDecimal()
            {
                Assert.Equal(9.5m, new DataNode("9.5").AsDecimal());
            }

            [Fact]
            public void AsDecimal_WithKeyPath_ReturnsDecimal()
            {
                Assert.Equal(9.5m, SampleMap().AsDecimal("score"));
            }

            [Fact]
            public void AsDecimalOrDefault_NullValue_ReturnsDefault()
            {
                Assert.Equal(0m, new DataNode(null).AsDecimalOrDefault(0m));
            }

            [Fact]
            public void AsDecimalOrDefault_InvalidString_ReturnsDefault()
            {
                Assert.Equal(-1m, new DataNode("invalid").AsDecimalOrDefault(-1m));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  15. AsDateTime / AsDateTimeOrDefault
        // ═════════════════════════════════════════════════════════════════════
        public class AsDateTimeMethods
        {
            [Fact]
            public void AsDateTime_ValidString_ReturnsDateTime()
            {
                var expected = new DateTime(2024, 1, 15);
                var node = new DataNode("2024-01-15");
                Assert.Equal(expected, node.AsDateTime());
            }

            [Fact]
            public void AsDateTime_WithKeyPath_ReturnsDateTime()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["date"] = "2024-01-15"
                });
                Assert.Equal(new DateTime(2024, 1, 15), node.AsDateTime("date"));
            }

            [Fact]
            public void AsDateTimeOrDefault_NullValue_ReturnsDefault()
            {
                var def = new DateTime(2000, 1, 1);
                var result = new DataNode(null).AsDateTimeOrDefault(def);
                Assert.Equal(def, result);
            }

            [Fact]
            public void AsDateTimeOrDefault_InvalidString_ReturnsDefault()
            {
                var def = new DateTime(2000, 1, 1);
                var result = new DataNode("notadate").AsDateTimeOrDefault(def);
                Assert.Equal(def, result);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  16. AsBool / AsBoolOrDefault
        // ═════════════════════════════════════════════════════════════════════
        public class AsBoolMethods
        {
            [Theory]
            [InlineData("true", true)]
            [InlineData("True", true)]
            [InlineData("TRUE", true)]
            [InlineData("1", true)]
            [InlineData("false", false)]
            [InlineData("False", false)]
            [InlineData("FALSE", false)]
            [InlineData("0", false)]
            public void AsBool_ValidStrings_ReturnsExpected(string input, bool expected)
            {
                Assert.Equal(expected, new DataNode(input).AsBool());
            }

            [Fact]
            public void AsBool_NativeBool_ReturnsValue()
            {
                Assert.True(new DataNode(true).AsBool());
                Assert.False(new DataNode(false).AsBool());
            }

            [Fact]
            public void AsBool_WithKeyPath_ReturnsValue()
            {
                Assert.True(SampleMap().AsBool("active"));
            }

            [Fact]
            public void AsBool_InvalidString_Throws()
            {
                Assert.Throws<FormatException>(() => new DataNode("maybe").AsBool());
            }

            [Fact]
            public void AsBoolOrDefault_InvalidString_ReturnsDefault()
            {
                Assert.False(new DataNode("maybe").AsBoolOrDefault(false));
            }

            [Fact]
            public void AsBoolOrDefault_NullValue_ReturnsDefault()
            {
                Assert.True(new DataNode(null).AsBoolOrDefault(true));
            }

            [Fact]
            public void AsBoolOrDefault_WithKeyPath_NonExisting_ReturnsDefault()
            {
                Assert.True(SampleMap().AsBoolOrDefault("nonexistent", true));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  17. Merge
        // ═════════════════════════════════════════════════════════════════════
        public class MergeMethod
        {
            [Fact]
            public void Merge_AddsNewKeys()
            {
                var node1 = new DataNode(new Dictionary<string, object> { ["a"] = "1" });
                var node2 = new DataNode(new Dictionary<string, object> { ["b"] = "2" });

                node1.Merge(node2);

                Assert.True(node1.Exists("a"));
                Assert.True(node1.Exists("b"));
            }

            [Fact]
            public void Merge_OverwritesExistingScalar()
            {
                var node1 = new DataNode(new Dictionary<string, object> { ["id"] = "1" });
                var node2 = new DataNode(new Dictionary<string, object> { ["id"] = "2" });

                node1.Merge(node2);

                Assert.Equal("2", node1.AsString("id"));
            }

            [Fact]
            public void Merge_NestedMaps_MergesRecursively()
            {
                var node1 = new DataNode(new Dictionary<string, object>
                {
                    ["user"] = new Dictionary<string, object>
                    {
                        ["name"] = "Ana",
                        ["email"] = "ana@email.com"
                    }
                });

                var node2 = new DataNode(new Dictionary<string, object>
                {
                    ["user"] = new Dictionary<string, object>
                    {
                        ["name"] = "Bob",
                        ["phone"] = "9999-9999"
                    }
                });

                node1.Merge(node2);

                // name sobrescrito
                Assert.Equal("Bob", node1.AsString("user.name"));
                // email preservado
                Assert.Equal("ana@email.com", node1.AsString("user.email"));
                // phone adicionado
                Assert.Equal("9999-9999", node1.AsString("user.phone"));
            }

            [Fact]
            public void Merge_OverwritesSequenceWithSequence()
            {
                var node1 = new DataNode(new Dictionary<string, object>
                {
                    ["tags"] = new List<object> { "a", "b" }
                });
                var node2 = new DataNode(new Dictionary<string, object>
                {
                    ["tags"] = new List<object> { "c", "d", "e" }
                });

                node1.Merge(node2);

                var tags = node1.GetSequenceNode("tags");
                Assert.Equal(3, tags.Count);
                Assert.Equal("c", tags[0].AsString());
            }

            [Fact]
            public void Merge_OverwritesMapWithSequence()
            {
                var node1 = new DataNode(new Dictionary<string, object>
                {
                    ["data"] = new Dictionary<string, object> { ["k"] = "v" }
                });
                var node2 = new DataNode(new Dictionary<string, object>
                {
                    ["data"] = new List<object> { "x", "y" }
                });

                node1.Merge(node2);

                Assert.Equal(NodeType.Sequence, node1.GetNode("data").Type);
            }

            [Fact]
            public void Merge_SampleScenario_IdAndNewKey()
            {
                var node1 = new DataNode(new Dictionary<string, object>
                {
                    ["Id"] = "1",
                    ["Ativo"] = "true"
                });
                var node2 = new DataNode(new Dictionary<string, object>
                {
                    ["Id"] = "2",
                    ["Data"] = new List<object> { "1", "2", "3" }
                });

                node1.Merge(node2);

                Assert.Equal("2", node1.AsString("Id"));
                Assert.Equal("true", node1.AsString("Ativo"));
                Assert.True(node1.Exists("Data"));
                Assert.Equal(3, node1.GetSequenceNode("Data").Count);
            }

            [Fact]
            public void Merge_NullOther_DoesNotThrow()
            {
                var node = new DataNode(new Dictionary<string, object> { ["k"] = "v" });
                node.Merge(null);
                Assert.Equal("v", node.AsString("k"));
            }

            [Fact]
            public void Merge_OtherHasNoValue_DoesNothing()
            {
                var node = new DataNode(new Dictionary<string, object> { ["k"] = "v" });
                var other = new DataNode(null);
                node.Merge(other);
                Assert.Equal("v", node.AsString("k"));
            }

            [Fact]
            public void Merge_OtherIsNotMap_DoesNothing()
            {
                var node = new DataNode(new Dictionary<string, object> { ["k"] = "v" });
                var other = new DataNode(new List<object> { "x" });
                node.Merge(other);
                Assert.Equal("v", node.AsString("k"));
            }

            [Fact]
            public void Merge_OnNonMap_Throws()
            {
                var node = new DataNode("scalar");
                var other = new DataNode(new Dictionary<string, object> { ["k"] = "v" });
                Assert.Throws<InvalidOperationException>(() => node.Merge(other));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  18. ToString
        // ═════════════════════════════════════════════════════════════════════
        public class ToStringMethod
        {
            [Fact]
            public void ToString_Scalar_ReturnsValue()
            {
                Assert.Equal("hello", new DataNode("hello").ToString());
            }

            [Fact]
            public void ToString_NullScalar_ReturnsNullLabel()
            {
                Assert.Equal("(null)", new DataNode(null).ToString());
            }

            [Fact]
            public void ToString_Sequence_ReturnsCountLabel()
            {
                var node = new DataNode(new List<object> { "a", "b", "c" });
                Assert.Equal("[Sequence, 3 items]", node.ToString());
            }

            [Fact]
            public void ToString_Map_ReturnsCountLabel()
            {
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["a"] = 1,
                    ["b"] = 2
                });
                Assert.Equal("[Map, 2 keys]", node.ToString());
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  19. CultureInfo
        // ═════════════════════════════════════════════════════════════════════
        public class CultureInfoBehavior
        {
            [Fact]
            public void AsDouble_WithPtBrCulture_ParsesCommaAsDecimalSeparator()
            {
                // InvariantCulture usa ponto; pt-BR usa vírgula
                // O valor "9,5" com InvariantCulture lançaria exceção
                var culture = new CultureInfo("pt-BR");
                var node = new DataNode("9,5", culture);
                Assert.Equal(9.5, node.AsDouble());
            }

            [Fact]
            public void GetNode_InheritsCultureFromParent()
            {
                var culture = new CultureInfo("pt-BR");
                var node = new DataNode(new Dictionary<string, object>
                {
                    ["value"] = "9,5"
                }, culture);
                Assert.Equal(culture, node.GetNode("value").Culture);
            }

            [Fact]
            public void AsDecimal_WithPtBrCulture_ParsesCorrectly()
            {
                var culture = new CultureInfo("pt-BR");
                var node = new DataNode("9,5", culture);
                Assert.Equal(9.5m, node.AsDecimal());
            }
        }
    }
}
