using Autossential.Activities.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Autossential.Activities.Tests.Core
{
    // ─────────────────────────────────────────────────────────────────────────
    //  DataNodeTests
    //
    //  Tests for DataNode: a normalized, immutable wrapper around values from
    //  YAML/JSON parsers or user-supplied objects.
    //
    //  Coverage areas:
    //    1. Construction and type detection
    //    2. Scalar accessors with type conversion
    //    3. Collection accessors (Map and Sequence)
    //    4. Key-path navigation (dots, brackets, quoted keys)
    //    5. Typed navigation with defaults
    //    6. Sequence retrieval
    //    7. Existence checks
    //    8. Edge cases and error handling
    // ─────────────────────────────────────────────────────────────────────────
    public class DataNodeTests
    {
        // ═════════════════════════════════════════════════════════════════════
        //  1. Construction and Type Detection
        // ═════════════════════════════════════════════════════════════════════
        public class ConstructionAndTypeDetection
        {
            [Fact]
            public void Null_CreatesScalarNode()
            {
                var node = new DataNode(null);
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.False(node.HasValue);
                Assert.Null(node.RawValue);
            }

            [Fact]
            public void String_CreatesScalarNode()
            {
                var node = new DataNode("hello");
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.True(node.HasValue);
                Assert.Equal("hello", node.RawValue);
            }

            [Fact]
            public void Integer_CreatesScalarNode()
            {
                var node = new DataNode(42);
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.True(node.HasValue);
                Assert.Equal(42, node.RawValue);
            }

            [Fact]
            public void Bool_CreatesScalarNode()
            {
                var node = new DataNode(true);
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.True(node.HasValue);
                Assert.Equal(true, node.RawValue);
            }

            [Fact]
            public void DateTime_CreatesScalarNode()
            {
                var dt = DateTime.Now;
                var node = new DataNode(dt);
                Assert.Equal(NodeType.Scalar, node.Type);
                Assert.True(node.HasValue);
                Assert.Equal(dt, node.RawValue);
            }

            [Fact]
            public void List_CreatesSequenceNode()
            {
                var list = new List<object> { "a", "b", "c" };
                var node = new DataNode(list);
                Assert.Equal(NodeType.Sequence, node.Type);
                Assert.True(node.HasValue);
            }

            [Fact]
            public void Array_CreatesSequenceNode()
            {
                var array = new object[] { 1, 2, 3 };
                var node = new DataNode(array);
                Assert.Equal(NodeType.Sequence, node.Type);
            }

            [Fact]
            public void Dictionary_CreatesMapNode()
            {
                var dict = new Dictionary<string, object> { { "key", "value" } };
                var node = new DataNode(dict);
                Assert.Equal(NodeType.Map, node.Type);
                Assert.True(node.HasValue);
            }

            [Fact]
            public void NestedDataNode_UnwrapsAndNormalizesValue()
            {
                var inner = new DataNode(42);
                var outer = new DataNode(inner);
                Assert.Equal(NodeType.Scalar, outer.Type);
                Assert.Equal(42, outer.RawValue);
            }

            [Fact]
            public void Hashtable_NormalizesToMapNode()
            {
                var hashtable = new System.Collections.Hashtable { { "key", "value" } };
                var node = new DataNode(hashtable);
                Assert.Equal(NodeType.Map, node.Type);
            }

            [Fact]
            public void NestedList_NormalizesChildrenRecursively()
            {
                var list = new List<object> { new List<object> { "a" }, "b" };
                var node = new DataNode(list);
                Assert.Equal(NodeType.Sequence, node.Type);
                var items = node.AsList();
                Assert.Equal(2, items.Count);
            }

            [Fact]
            public void NestedDictionary_NormalizesValuesRecursively()
            {
                var dict = new Dictionary<string, object>
                {
                    { "nested", new Dictionary<string, object> { { "key", "value" } } }
                };
                var node = new DataNode(dict);
                var nested = node.AsDictionary()["nested"];
                Assert.IsType<Dictionary<string, object>>(nested);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  2. Scalar Accessors (No Key Path)
        // ═════════════════════════════════════════════════════════════════════
        public class ScalarAccessors
        {
            [Fact]
            public void AsString_ReturnsStringValue()
            {
                var node = new DataNode("hello");
                Assert.Equal("hello", node.AsString());
            }

            [Fact]
            public void AsString_ReturnsNullForNullValue()
            {
                var node = new DataNode(null);
                Assert.Null(node.AsString());
            }

            [Fact]
            public void AsString_ConvertsIntegerToString()
            {
                var node = new DataNode(42);
                Assert.Equal("42", node.AsString());
            }

            [Fact]
            public void AsStringOrDefault_ReturnsStringValue()
            {
                var node = new DataNode("hello");
                Assert.Equal("hello", node.AsStringOrDefault("default"));
            }

            [Fact]
            public void AsStringOrDefault_ReturnsDefaultForNull()
            {
                var node = new DataNode(null);
                Assert.Equal("default", node.AsStringOrDefault("default"));
            }

            [Fact]
            public void AsInt_ReturnsIntegerValue()
            {
                var node = new DataNode(42);
                Assert.Equal(42, node.AsInt());
            }

            [Fact]
            public void AsInt_ParsesStringInteger()
            {
                var node = new DataNode("42");
                Assert.Equal(42, node.AsInt());
            }

            [Fact]
            public void AsInt_ReturnsDefaultOnInvalidString()
            {
                var node = new DataNode("not-a-number");
                Assert.Equal(99, node.AsInt(99));
            }

            [Fact]
            public void AsInt_ReturnsDefaultForNull()
            {
                var node = new DataNode(null);
                Assert.Equal(0, node.AsInt());
            }

            [Fact]
            public void AsLong_ReturnsLongValue()
            {
                var node = new DataNode(9999999999L);
                Assert.Equal(9999999999L, node.AsLong());
            }

            [Fact]
            public void AsDouble_ReturnsDoubleValue()
            {
                var node = new DataNode(3.14);
                Assert.Equal(3.14, node.AsDouble());
            }

            [Fact]
            public void AsDouble_ParsesStringDouble()
            {
                var node = new DataNode("3.14");
                // Convert.ToDouble("3.14") works correctly
                Assert.Equal(3.14, node.AsDouble(), precision: 10);
            }

            [Fact]
            public void AsBool_ReturnsBoolValue()
            {
                var node = new DataNode(true);
                Assert.True(node.AsBool());
            }

            [Fact]
            public void AsBool_ParsesTrueString()
            {
                var node = new DataNode("true");
                Assert.True(node.AsBool());
            }

            [Fact]
            public void AsBool_ParsesTrueVariants()
            {
                foreach (var value in new[] { "true", "yes", "on", "1" })
                {
                    var node = new DataNode(value);
                    Assert.True(node.AsBool(), $"Failed for value: {value}");
                }
            }

            [Fact]
            public void AsBool_ParsesFalseVariants()
            {
                foreach (var value in new[] { "false", "no", "off", "0" })
                {
                    var node = new DataNode(value);
                    Assert.False(node.AsBool(), $"Failed for value: {value}");
                }
            }

            [Fact]
            public void AsBool_ReturnsDefaultForInvalidString()
            {
                var node = new DataNode("maybe");
                Assert.False(node.AsBool(false));
            }

            [Fact]
            public void AsDateTime_ReturnsDateTimeValue()
            {
                var dt = new DateTime(2024, 1, 15);
                var node = new DataNode(dt);
                Assert.Equal(dt, node.AsDateTime());
            }

            [Fact]
            public void AsDecimal_ReturnsDecimalValue()
            {
                var value = 99.99m;
                var node = new DataNode(value);
                Assert.Equal(value, node.AsDecimal());
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  3. Collection Accessors
        // ═════════════════════════════════════════════════════════════════════
        public class CollectionAccessors
        {
            [Fact]
            public void AsDictionary_ReturnsDictionaryForMapNode()
            {
                var dict = new Dictionary<string, object> { { "key", "value" } };
                var node = new DataNode(dict);
                var result = node.AsDictionary();
                Assert.Single(result);
                Assert.Equal("value", result["key"]);
            }

            [Fact]
            public void AsDictionary_ThrowsForNonMapNode()
            {
                var node = new DataNode("not a map");
                Assert.Throws<InvalidOperationException>(() => node.AsDictionary());
            }

            [Fact]
            public void AsList_ReturnsListForSequenceNode()
            {
                var list = new List<object> { "a", "b", "c" };
                var node = new DataNode(list);
                var result = node.AsList();
                Assert.Equal(3, result.Count);
            }

            [Fact]
            public void AsList_ThrowsForNonSequenceNode()
            {
                var node = new DataNode("not a list");
                Assert.Throws<InvalidOperationException>(() => node.AsList());
            }

            [Fact]
            public void AsDictionary_NormalizesNestedValues()
            {
                var dict = new Dictionary<string, object>
                {
                    { "items", new List<object> { 1, 2, 3 } }
                };
                var node = new DataNode(dict);
                var result = node.AsDictionary();
                Assert.IsType<List<object>>(result["items"]);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  4. Key-Path Navigation
        // ═════════════════════════════════════════════════════════════════════
        public class KeyPathNavigation
        {
            private DataNode CreateSampleData()
            {
                var data = new Dictionary<string, object>
                {
                    { "person", new Dictionary<string, object>
                        {
                            { "name", "Alice" },
                            { "age", "30" },
                            { "address", new Dictionary<string, object>
                                {
                                    { "city", "Curitiba" },
                                    { "state", "PR" }
                                }
                            }
                        }
                    },
                    { "servers", new List<object>
                        {
                            new Dictionary<string, object> { { "host", "server1" }, { "port", "8080" } },
                            new Dictionary<string, object> { { "host", "server2" }, { "port", "8081" } }
                        }
                    }
                };
                return new DataNode(data);
            }

        }

        // ═════════════════════════════════════════════════════════════════════
        //  5. Typed Navigation with Defaults
        // ═════════════════════════════════════════════════════════════════════
        public class TypedNavigation
        {
            private DataNode CreateSampleData()
            {
                return new DataNode(new Dictionary<string, object>
                {
                    { "name", "Bob" },
                    { "age", "25" },
                    { "score", "98.5" },
                    { "active", "true" },
                    { "registered", new DateTime(2023, 1, 1) }
                });
            }

            [Fact]
            public void AsString_WithPath()
            {
                var data = CreateSampleData();
                var result = data.AsString("name");
                Assert.Equal("Bob", result);
            }

            [Fact]
            public void AsString_WithPath_ReturnsDefaultForMissing()
            {
                var data = CreateSampleData();
                var result = data.AsString("missing.key", "default");
                Assert.Equal("default", result);
            }

            [Fact]
            public void AsInt_WithPath()
            {
                var data = CreateSampleData();
                var result = data.AsInt("age");
                Assert.Equal(25, result);
            }

            [Fact]
            public void AsInt_WithPath_ReturnsDefaultForMissing()
            {
                var data = CreateSampleData();
                var result = data.AsInt("missing.age", 99);
                Assert.Equal(99, result);
            }

            [Fact]
            public void AsDouble_WithPath()
            {
                var data = CreateSampleData();
                // "98.5" string conversion through the conversion helper
                var result = data.AsDouble("score");
                Assert.Equal(98.5, result);
            }

            [Fact]
            public void AsBool_WithPath()
            {
                var data = CreateSampleData();
                var result = data.AsBool("active");
                Assert.True(result);
            }

            [Fact]
            public void AsDateTime_WithPath()
            {
                var data = CreateSampleData();
                var result = data.AsDateTime("registered");
                Assert.Equal(new DateTime(2023, 1, 1), result);
            }

            [Fact]
            public void AsDecimal_WithPath()
            {
                var data = new DataNode(new Dictionary<string, object> { { "price", "19.99" } });
                // String "19.99" converts to decimal
                var result = data.AsDecimal("price");
                Assert.Equal(19.99m, result);
            }

            [Fact]
            public void AsLong_WithPath()
            {
                var data = new DataNode(new Dictionary<string, object> { { "bignum", "9999999999" } });
                var result = data.AsLong("bignum");
                Assert.Equal(9999999999L, result);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  6. Get and GetSequence Methods
        // ═════════════════════════════════════════════════════════════════════
        public class GetMethods
        {
            private DataNode CreateSampleData()
            {
                return new DataNode(new Dictionary<string, object>
                {
                    { "address", new Dictionary<string, object>
                        {
                            { "city", "Curitiba" },
                            { "state", "PR" }
                        }
                    },
                    { "tags", new List<object> { "admin", "user", "developer" } }
                });
            }

            [Fact]
            public void Get_ReturnsDataNodeForNavigation()
            {
                var data = CreateSampleData();
                var result = data.Get("address");
                Assert.Equal(NodeType.Map, result.Type);
            }

            [Fact]
            public void GetSequence_ReturnsListOfDataNodes()
            {
                var data = CreateSampleData();
                var result = data.GetSequence("tags");
                Assert.Equal(3, result.Count);
                Assert.Equal("admin", result[0].AsString());
                Assert.Equal("user", result[1].AsString());
                Assert.Equal("developer", result[2].AsString());
            }

            [Fact]
            public void GetSequence_ReturnsEmptyForNonsequence()
            {
                var data = CreateSampleData();
                var result = data.GetSequence("address");
                Assert.Empty(result);
            }

            [Fact]
            public void GetSequence_ReturnsEmptyForMissingPath()
            {
                var data = CreateSampleData();
                var result = data.GetSequence("missing.tags");
                Assert.Empty(result);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  7. Exists Method
        // ═════════════════════════════════════════════════════════════════════
        public class ExistsMethods
        {
            private DataNode CreateSampleData()
            {
                return new DataNode(new Dictionary<string, object>
                {
                    { "person", new Dictionary<string, object>
                        {
                            { "name", "Alice" },
                            { "age", "30" }
                        }
                    },
                    { "empty", null! }
                });
            }

            [Fact]
            public void Exists_ReturnsTrueForExistingKey()
            {
                var data = CreateSampleData();
                Assert.True(data.Exists("person.name"));
            }

            [Fact]
            public void Exists_ReturnsFalseForMissingKey()
            {
                var data = CreateSampleData();
                Assert.False(data.Exists("person.missing"));
            }

            [Fact]
            public void Exists_ReturnsFalseForNullValue()
            {
                var data = CreateSampleData();
                Assert.False(data.Exists("empty"));
            }

            [Fact]
            public void Exists_ReturnsFalseForInvalidPath()
            {
                var data = CreateSampleData();
                Assert.False(data.Exists("person[0].name"));
            }

            [Fact]
            public void Exists_HandlesCatchingInvalidOperations()
            {
                var data = CreateSampleData();
                var result = data.Exists("person[0].name");
                Assert.False(result);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  8. ToString
        // ═════════════════════════════════════════════════════════════════════
        public class ToStringTests
        {
            [Fact]
            public void ToString_ScalarNull()
            {
                var node = new DataNode(null);
                Assert.Equal("(null)", node.ToString());
            }

            [Fact]
            public void ToString_ScalarValue()
            {
                var node = new DataNode("hello");
                Assert.Equal("hello", node.ToString());
            }

            [Fact]
            public void ToString_Sequence()
            {
                var node = new DataNode(new List<object> { 1, 2, 3 });
                Assert.Contains("Sequence", node.ToString());
                Assert.Contains("3", node.ToString());
            }

            [Fact]
            public void ToString_Map()
            {
                var node = new DataNode(new Dictionary<string, object> { { "key", "value" } });
                Assert.Contains("Map", node.ToString());
                Assert.Contains("1", node.ToString());
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  9. Edge Cases
        // ═════════════════════════════════════════════════════════════════════
        public class EdgeCases
        {
            [Fact]
            public void EmptyList_CreatesSequenceNode()
            {
                var node = new DataNode(new List<object>());
                Assert.Equal(NodeType.Sequence, node.Type);
                Assert.Empty(node.AsList());
            }

            [Fact]
            public void EmptyDictionary_CreatesMapNode()
            {
                var node = new DataNode(new Dictionary<string, object>());
                Assert.Equal(NodeType.Map, node.Type);
                Assert.Empty(node.AsDictionary());
            }

            [Fact]
            public void ListWithNullElements_NormalizesCorrectly()
            {
                var node = new DataNode(new List<object> { "a", null!, "c" });
                var list = node.AsList();
                Assert.Equal(3, list.Count);
                Assert.Null(list[1]);
            }

            [Fact]
            public void DictionaryWithNullValues_NormalizesCorrectly()
            {
                var dict = new Dictionary<string, object> { { "key", null! } };
                var node = new DataNode(dict);
                var result = node.AsDictionary();
                Assert.Null(result["key"]);
            }

            [Fact]
            public void DictionaryKeyConversion_ToStringsOrdinally()
            {
                var dict = new System.Collections.Hashtable { { 123, "value" } };
                var node = new DataNode(dict);
                var result = node.AsDictionary();
                Assert.True(result.ContainsKey("123"));
            }

            [Fact]
            public void LargeNestedStructure_NormalizesCorrectly()
            {
                var data = new Dictionary<string, object>();
                for (int i = 0; i < 100; i++)
                {
                    data[$"key{i}"] = new List<object> { i, i * 2, i * 3 };
                }
                var node = new DataNode(data);
                Assert.Equal(100, node.AsDictionary().Count);
            }
        }
    }
}
