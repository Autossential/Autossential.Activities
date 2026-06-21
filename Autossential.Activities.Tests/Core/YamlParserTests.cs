using Autossential.Activities.Core;

namespace Autossential.Activities.Tests.Core
{
    public class YamlParserTests
    {

        [Test]
        public async Task TestBasic_ShouldParseCorrectly()
        {
            string yaml = TestBasic();
            var result = YamlParser.Parse(yaml);

            var dict = await Assert.That(result).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(dict).IsNotNull();
            await Assert.That(dict["name"]).IsEqualTo("Alice");
            await Assert.That(dict["age"]).IsEqualTo("30");      // número tratado como string
            await Assert.That(dict["active"]).IsEqualTo("true"); // boolean tratado como string

            var tags = await Assert.That(dict["tags"]).IsTypeOf<List<object>>();
            await Assert.That(tags).Contains("admin");
            await Assert.That(tags).Contains("user");

            var address = await Assert.That(dict["address"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(address).IsNotNull();
            await Assert.That(address["city"]).IsEqualTo("Curitiba");
            await Assert.That(address["state"]).IsEqualTo("PR");
        }

        [Test]
        public async Task TestBlockScalars_ShouldParseCorrectly()
        {
            string yaml = TestBlockScalars();
            var result = YamlParser.Parse(yaml);

            var dict = await Assert.That(result).IsTypeOf<Dictionary<string, object>>();

            await Assert.That(dict).IsNotNull();
            await Assert.That(dict["literal"].ToString()).Contains("Hello,");
            await Assert.That(dict["folded"].ToString()).Contains("single line");
            await Assert.That(dict["chomp_strip"].ToString()).Contains("No trailing newline");
        }

        [Test]
        public async Task TestAnchors_ShouldParseCorrectly()
        {
            string yaml = TestAnchors();
            var result = YamlParser.Parse(yaml);

            var dict = await Assert.That(result).IsTypeOf<Dictionary<string, object>>();

            var defaults = await Assert.That(dict["defaults"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(defaults["adapter"]).IsEqualTo("postgres");

            var test = await Assert.That(dict["test"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(test["database"]).IsEqualTo("myapp_test");
        }

        [Test]
        public async Task TestMergeKeys_ShouldParseCorrectly()
        {
            string yaml = TestMergeKeys();
            var result = YamlParser.Parse(yaml);

            var dict = await Assert.That(result).IsTypeOf<Dictionary<string, object>>();

            var baseDict = await Assert.That(dict["base"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(baseDict["x"]).IsEqualTo("1");   // scalar tratado como string

            var extended = await Assert.That(dict["extended"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(extended["color"]).IsEqualTo("blue");
        }

        [Test]
        public async Task TestDockerCompose_ShouldParseCorrectly()
        {
            string yaml = TestDockerCompose();
            var result = YamlParser.Parse(yaml);

            var dict = await Assert.That(result).IsTypeOf<Dictionary<string, object>>();

            var commonEnv = await Assert.That(dict["x-common-env"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(commonEnv["DB_HOST"]).IsEqualTo("postgres");

            var services = await Assert.That(dict["services"]).IsTypeOf<Dictionary<string, object>>();
            var web = await Assert.That(services["web"]).IsTypeOf<Dictionary<string, object>>();
            var envWeb = await Assert.That(web["environment"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(envWeb["RAILS_ENV"]).IsEqualTo("production");
        }

        [Test]
        public async Task TestKubernetes_ShouldParseCorrectly()
        {
            string yaml = TestKubernetes();
            var result = YamlParser.Parse(yaml);

            var dict = await Assert.That(result).IsTypeOf<Dictionary<string, object>>();

            await Assert.That(dict["kind"]).IsEqualTo("ConfigMap");

            var metadata = await Assert.That(dict["metadata"]).IsTypeOf<Dictionary<string, object>>();
            await Assert.That(metadata["name"]).IsEqualTo("nginx-config");
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