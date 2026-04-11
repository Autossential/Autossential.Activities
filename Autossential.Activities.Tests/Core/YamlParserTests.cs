using Autossential.Activities.Core;
using Xunit;

namespace Autossential.Activities.Tests.Core
{
    public class YamlParserTests
    {
        [Fact]
        public void TestBasic_ShouldParseCorrectly()
        {
            string yaml = TestBasic();
            var result = YamlParser.Parse(yaml);

            var dict = Assert.IsType<Dictionary<string, object>>(result);

            Assert.Equal("Alice", dict["name"]);
            Assert.Equal("30", dict["age"]);          // número tratado como string
            Assert.Equal("true", dict["active"]);     // boolean tratado como string

            var tags = Assert.IsType<List<object>>(dict["tags"]);
            Assert.Contains("admin", tags);
            Assert.Contains("user", tags);

            var address = Assert.IsType<Dictionary<string, object>>(dict["address"]);
            Assert.Equal("Curitiba", address["city"]);
            Assert.Equal("PR", address["state"]);
        }

        [Fact]
        public void TestBlockScalars_ShouldParseCorrectly()
        {
            string yaml = TestBlockScalars();
            var result = YamlParser.Parse(yaml);

            var dict = Assert.IsType<Dictionary<string, object>>(result);

            Assert.Contains("Hello,", dict["literal"].ToString());
            Assert.Contains("single line", dict["folded"].ToString());
            Assert.Contains("No trailing newline", dict["chomp_strip"].ToString());
        }

        [Fact]
        public void TestAnchors_ShouldParseCorrectly()
        {
            string yaml = TestAnchors();
            var result = YamlParser.Parse(yaml);

            var dict = Assert.IsType<Dictionary<string, object>>(result);

            var defaults = Assert.IsType<Dictionary<string, object>>(dict["defaults"]);
            Assert.Equal("postgres", defaults["adapter"]);

            var test = Assert.IsType<Dictionary<string, object>>(dict["test"]);
            Assert.Equal("myapp_test", test["database"]);
        }

        [Fact]
        public void TestMergeKeys_ShouldParseCorrectly()
        {
            string yaml = TestMergeKeys();
            var result = YamlParser.Parse(yaml);

            var dict = Assert.IsType<Dictionary<string, object>>(result);

            var baseDict = Assert.IsType<Dictionary<string, object>>(dict["base"]);
            Assert.Equal("1", baseDict["x"]);   // scalar tratado como string

            var extended = Assert.IsType<Dictionary<string, object>>(dict["extended"]);
            Assert.Equal("blue", extended["color"]);
        }

        [Fact]
        public void TestDockerCompose_ShouldParseCorrectly()
        {
            string yaml = TestDockerCompose();
            var result = YamlParser.Parse(yaml);

            var dict = Assert.IsType<Dictionary<string, object>>(result);

            var commonEnv = Assert.IsType<Dictionary<string, object>>(dict["x-common-env"]);
            Assert.Equal("postgres", commonEnv["DB_HOST"]);

            var services = Assert.IsType<Dictionary<string, object>>(dict["services"]);
            var web = Assert.IsType<Dictionary<string, object>>(services["web"]);
            var envWeb = Assert.IsType<Dictionary<string, object>>(web["environment"]);
            Assert.Equal("production", envWeb["RAILS_ENV"]);
        }

        [Fact]
        public void TestKubernetes_ShouldParseCorrectly()
        {
            string yaml = TestKubernetes();
            var result = YamlParser.Parse(yaml);

            var dict = Assert.IsType<Dictionary<string, object>>(result);

            Assert.Equal("ConfigMap", dict["kind"]);

            var metadata = Assert.IsType<Dictionary<string, object>>(dict["metadata"]);
            Assert.Equal("nginx-config", metadata["name"]);
        }

        // Métodos auxiliares com os YAMLs originais
        static string TestBasic() => """
        name: Alice
        age: 30
        active: true

        tags:
          - admin
          - user

        address:
          city: Curitiba
          state: PR
        """;

        static string TestBlockScalars() => """
        literal: |
          Hello,
          World!
          This preserves newlines.
        folded: >
          This long sentence
          is folded into
          a single line.
        chomp_strip: |-
          No trailing newline here
        """;

        static string TestAnchors() => """
        defaults: &defaults
          adapter: postgres
          encoding: utf8
          pool: 5

        development:
          <<: *defaults
          database: myapp_development

        test:
          <<: *defaults
          database: myapp_test
        """;

        static string TestMergeKeys() => """
        base: &base
          x: 1
          y: 2
          color: red

        extended:
          <<: *base
          z: 3
          color: blue  # overrides red
        """;

        static string TestDockerCompose() => """
        version: "3.8"

        x-common-env: &common-env
          DB_HOST: postgres
          DB_PORT: "5432"
          DB_NAME: myapp

        services:
          web:
            image: myapp:latest
            environment:
              <<: *common-env
              RAILS_ENV: production
            ports:
              - "3000:3000"

          worker:
            image: myapp:latest
            environment:
              <<: *common-env
              QUEUE: default
            command: bundle exec sidekiq
        """;

        static string TestKubernetes() => """
        apiVersion: v1
        kind: ConfigMap
        metadata:
          name: nginx-config
          namespace: production
        data:
          nginx.conf: |
            server {
                listen 80;
                server_name example.com;

                location / {
                    proxy_pass http://backend:8080;
                    proxy_set_header Host $host;
                }
            }
          app.properties: |
            spring.datasource.url=jdbc:postgresql://db:5432/mydb
            spring.datasource.username=admin
            spring.jpa.hibernate.ddl-auto=validate
        """;
    }
}