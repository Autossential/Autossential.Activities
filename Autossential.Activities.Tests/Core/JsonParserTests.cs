using Autossential.Activities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Autossential.Activities.Tests.Core
{
    // ─────────────────────────────────────────────────────────────────────────
    //  SimpleJsonParserTests
    //
    //  Mirrors the six test cases from SimpleYamlParser:
    //    1. Scalars, Lists and Maps
    //    2. Block text (multiline strings — JSON uses \n escapes)
    //    3. Anchors & Aliases  → JSON has no anchors; equivalent is shared
    //       objects referenced by value (we test repeated identical structure)
    //    4. Merge / object spread → represented as a flat merged object in JSON
    //    5. Docker-Compose-style (services with shared environment)
    //    6. Kubernetes ConfigMap with embedded multiline scripts
    // ─────────────────────────────────────────────────────────────────────────
    public class JsonParserTests
    {
        // ── Helper ────────────────────────────────────────────────────────────

        private static Dictionary<string, object> ParseMap(string json) =>
            (Dictionary<string, object>)JsonParser.Parse(json);

        // ═════════════════════════════════════════════════════════════════════
        //  1. Scalars, Lists and Maps
        // ═════════════════════════════════════════════════════════════════════
        public class ScalarsListsAndMaps
        {
            private const string Json = """
                {
                    "name": "Alice",
                    "age": "30",
                    "active": "true",
                    "tags": ["admin", "user"],
                    "address": {
                        "city": "Curitiba",
                        "state": "PR"
                    }
                }
                """;

            [Fact]
            public void Root_IsMap()
            {
                object result = JsonParser.Parse(Json);
                Assert.IsType<Dictionary<string, object>>(result);
            }

            [Fact]
            public void Scalar_String()
            {
                var map = ParseMap(Json);
                Assert.Equal("Alice", map["name"]);
            }

            [Fact]
            public void Scalar_Number_ReturnedAsString()
            {
                var map = ParseMap(Json);
                Assert.Equal("30", map["age"]);
            }

            [Fact]
            public void Scalar_Boolean_ReturnedAsString()
            {
                var map = ParseMap(Json);
                Assert.Equal("true", map["active"]);
            }

            [Fact]
            public void List_CorrectType()
            {
                var map = ParseMap(Json);
                Assert.IsType<List<object>>(map["tags"]);
            }

            [Fact]
            public void List_CorrectItems()
            {
                var map = ParseMap(Json);
                var tags = (List<object>)map["tags"];
                Assert.Equal(2, tags.Count);
                Assert.Equal("admin", tags[0]);
                Assert.Equal("user", tags[1]);
            }

            [Fact]
            public void NestedMap_CorrectType()
            {
                var map = ParseMap(Json);
                Assert.IsType<Dictionary<string, object>>(map["address"]);
            }

            [Fact]
            public void NestedMap_CorrectValues()
            {
                var map = ParseMap(Json);
                var address = (Dictionary<string, object>)map["address"];
                Assert.Equal("Curitiba", address["city"]);
                Assert.Equal("PR", address["state"]);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  2. Multiline strings (JSON equivalent of block scalars | and >)
        // ═════════════════════════════════════════════════════════════════════
        public class MultilineStrings
        {
            private const string Json = """
                {
                    "literal": "Hello,\nWorld!\nThis preserves newlines.\n",
                    "folded":  "This long sentence is folded into a single line.",
                    "chomp_strip": "No trailing newline here"
                }
                """;

            [Fact]
            public void Literal_PreservesNewlines()
            {
                var map = ParseMap(Json);
                string value = (string)map["literal"];
                Assert.Contains('\n', value);
                Assert.StartsWith("Hello,", value);
            }

            [Fact]
            public void Folded_IsSingleLine()
            {
                var map = ParseMap(Json);
                string value = (string)map["folded"];
                Assert.DoesNotContain('\n', value);
                Assert.Contains("single line", value);
            }

            [Fact]
            public void ChompStrip_NoTrailingNewline()
            {
                var map = ParseMap(Json);
                string value = (string)map["chomp_strip"];
                Assert.False(value.EndsWith('\n'));
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  3. Shared structure (JSON equivalent of anchors & aliases)
        //     JSON has no anchor syntax — shared config is inlined explicitly.
        // ═════════════════════════════════════════════════════════════════════
        public class SharedStructure
        {
            private const string Json = """
                {
                    "defaults": {
                        "adapter":  "postgres",
                        "encoding": "utf8",
                        "pool":     "5"
                    },
                    "development": {
                        "adapter":  "postgres",
                        "encoding": "utf8",
                        "pool":     "5",
                        "database": "myapp_development"
                    },
                    "test": {
                        "adapter":  "postgres",
                        "encoding": "utf8",
                        "pool":     "5",
                        "database": "myapp_test"
                    }
                }
                """;

            [Fact]
            public void Defaults_HasExpectedKeys()
            {
                var map = ParseMap(Json);
                var defaults = (Dictionary<string, object>)map["defaults"];
                Assert.Equal("postgres", defaults["adapter"]);
                Assert.Equal("utf8", defaults["encoding"]);
                Assert.Equal("5", defaults["pool"]);
            }

            [Fact]
            public void Development_InheritsDefaults()
            {
                var map = ParseMap(Json);
                var dev = (Dictionary<string, object>)map["development"];
                Assert.Equal("postgres", dev["adapter"]);
                Assert.Equal("myapp_development", dev["database"]);
            }

            [Fact]
            public void Test_InheritsDefaults()
            {
                var map = ParseMap(Json);
                var test = (Dictionary<string, object>)map["test"];
                Assert.Equal("postgres", test["adapter"]);
                Assert.Equal("myapp_test", test["database"]);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  4. Merged objects (JSON equivalent of merge keys <<)
        //     In JSON, merging is expressed by explicitly listing all keys.
        // ═════════════════════════════════════════════════════════════════════
        public class MergedObjects
        {
            private const string Json = """
                {
                    "base": {
                        "x": "1",
                        "y": "2",
                        "color": "red"
                    },
                    "extended": {
                        "x": "1",
                        "y": "2",
                        "color": "blue",
                        "z": "3"
                    }
                }
                """;

            [Fact]
            public void Extended_InheritsBaseKeys()
            {
                var map = ParseMap(Json);
                var extended = (Dictionary<string, object>)map["extended"];
                Assert.Equal("1", extended["x"]);
                Assert.Equal("2", extended["y"]);
            }

            [Fact]
            public void Extended_OverridesColor()
            {
                var map = ParseMap(Json);
                var extended = (Dictionary<string, object>)map["extended"];
                Assert.Equal("blue", extended["color"]);
            }

            [Fact]
            public void Extended_HasOwnKey()
            {
                var map = ParseMap(Json);
                var extended = (Dictionary<string, object>)map["extended"];
                Assert.Equal("3", extended["z"]);
            }

            [Fact]
            public void Base_ColorUnchanged()
            {
                var map = ParseMap(Json);
                var base_ = (Dictionary<string, object>)map["base"];
                Assert.Equal("red", base_["color"]);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  5. Docker-Compose-style
        // ═════════════════════════════════════════════════════════════════════
        public class DockerComposeStyle
        {
            private const string Json = """
                {
                    "version": "3.8",
                    "services": {
                        "web": {
                            "image": "myapp:latest",
                            "environment": {
                                "DB_HOST":    "postgres",
                                "DB_PORT":    "5432",
                                "DB_NAME":    "myapp",
                                "RAILS_ENV":  "production"
                            },
                            "ports": ["3000:3000"]
                        },
                        "worker": {
                            "image": "myapp:latest",
                            "environment": {
                                "DB_HOST": "postgres",
                                "DB_PORT": "5432",
                                "DB_NAME": "myapp",
                                "QUEUE":   "default"
                            },
                            "command": "bundle exec sidekiq"
                        }
                    }
                }
                """;

            [Fact]
            public void Version_IsString()
            {
                var map = ParseMap(Json);
                Assert.Equal("3.8", map["version"]);
            }

            [Fact]
            public void Web_HasCorrectImage()
            {
                var map = ParseMap(Json);
                var services = (Dictionary<string, object>)map["services"];
                var web = (Dictionary<string, object>)services["web"];
                Assert.Equal("myapp:latest", web["image"]);
            }

            [Fact]
            public void Web_EnvironmentContainsSharedAndOwnVars()
            {
                var map = ParseMap(Json);
                var services = (Dictionary<string, object>)map["services"];
                var web = (Dictionary<string, object>)services["web"];
                var env = (Dictionary<string, object>)web["environment"];
                Assert.Equal("postgres", env["DB_HOST"]);
                Assert.Equal("production", env["RAILS_ENV"]);
            }

            [Fact]
            public void Web_Ports_IsList()
            {
                var map = ParseMap(Json);
                var services = (Dictionary<string, object>)map["services"];
                var web = (Dictionary<string, object>)services["web"];
                var ports = (List<object>)web["ports"];
                Assert.Single(ports);
                Assert.Equal("3000:3000", ports[0]);
            }

            [Fact]
            public void Worker_EnvironmentContainsSharedAndOwnVars()
            {
                var map = ParseMap(Json);
                var services = (Dictionary<string, object>)map["services"];
                var worker = (Dictionary<string, object>)services["worker"];
                var env = (Dictionary<string, object>)worker["environment"];
                Assert.Equal("postgres", env["DB_HOST"]);
                Assert.Equal("default", env["QUEUE"]);
            }

            [Fact]
            public void Worker_HasCommand()
            {
                var map = ParseMap(Json);
                var services = (Dictionary<string, object>)map["services"];
                var worker = (Dictionary<string, object>)services["worker"];
                Assert.Equal("bundle exec sidekiq", worker["command"]);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  6. Kubernetes ConfigMap with embedded multiline scripts
        // ═════════════════════════════════════════════════════════════════════
        public class KubernetesConfigMap
        {
            private const string Json = """
                {
                    "apiVersion": "v1",
                    "kind": "ConfigMap",
                    "metadata": {
                        "name":      "nginx-config",
                        "namespace": "production"
                    },
                    "data": {
                        "nginx.conf": "server {\n    listen 80;\n    server_name example.com;\n\n    location / {\n        proxy_pass http://backend:8080;\n        proxy_set_header Host $host;\n    }\n}\n",
                        "app.properties": "spring.datasource.url=jdbc:postgresql://db:5432/mydb\nspring.datasource.username=admin\nspring.jpa.hibernate.ddl-auto=validate\n"
                    }
                }
                """;

            [Fact]
            public void ApiVersion_IsCorrect()
            {
                var map = ParseMap(Json);
                Assert.Equal("v1", map["apiVersion"]);
            }

            [Fact]
            public void Kind_IsConfigMap()
            {
                var map = ParseMap(Json);
                Assert.Equal("ConfigMap", map["kind"]);
            }

            [Fact]
            public void Metadata_HasNameAndNamespace()
            {
                var map = ParseMap(Json);
                var metadata = (Dictionary<string, object>)map["metadata"];
                Assert.Equal("nginx-config", metadata["name"]);
                Assert.Equal("production", metadata["namespace"]);
            }

            [Fact]
            public void NginxConf_ContainsServerBlock()
            {
                var map = ParseMap(Json);
                var data = (Dictionary<string, object>)map["data"];
                string nginx = (string)data["nginx.conf"];
                Assert.Contains("listen 80", nginx);
                Assert.Contains("proxy_pass", nginx);
                Assert.Contains("server_name example", nginx);
            }

            [Fact]
            public void NginxConf_PreservesNewlines()
            {
                var map = ParseMap(Json);
                var data = (Dictionary<string, object>)map["data"];
                string nginx = (string)data["nginx.conf"];
                Assert.Contains('\n', nginx);
            }

            [Fact]
            public void AppProperties_ContainsSpringConfig()
            {
                var map = ParseMap(Json);
                var data = (Dictionary<string, object>)map["data"];
                string props = (string)data["app.properties"];
                Assert.Contains("jdbc:postgresql", props);
                Assert.Contains("spring.datasource.username", props);
                Assert.Contains("ddl-auto=validate", props);
            }

            [Fact]
            public void Null_JsonNull_ReturnsNull()
            {
                const string json = """{ "key": null }""";
                var map = ParseMap(json);
                Assert.Null(map["key"]);
            }
        }
    }
}
