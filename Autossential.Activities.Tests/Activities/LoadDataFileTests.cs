using System.Activities;
using System.Globalization;
using System.Text;

namespace Autossential.Activities.Tests.Activities
{
    public class LoadDataFileTests : BaseTests
    {
        [Test]
        public async Task WithValidJsonFile_LoadsAndParsesJson()
        {
            var dir = NewDir();

            var filePath = Path.Combine(dir, "data.json");
            var jsonContent = "{\"name\": \"Alice\", \"age\": 30}";
            File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = filePath
            };

            var result = WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

            await Assert.That(result).IsNotNull();
            await Assert.That(result["name"].AsString()).IsEqualTo("Alice");
        }

        [Test]
        public async Task WithValidYamlFile_LoadsAndParsesYaml()
        {
            var dir = NewDir();

            var filePath = Path.Combine(dir, "data.yaml");
            var yamlContent = "name: Bob\nage: 25";
            File.WriteAllText(filePath, yamlContent, Encoding.UTF8);

            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = filePath
            };

            var result = WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task WithNonExistentFile_ThrowsFileNotFoundException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = "/nonexistent/path/file.json"
            };

            await Assert.That(() => WorkflowInvoker.Invoke(new LoadDataFile(), inputs))
                .Throws<FileNotFoundException>();
        }

        [Test]
        public async Task WithCustomEncoding_LoadsWithSpecifiedEncoding()
        {
            var dir = NewDir();

            var filePath = Path.Combine(dir, "data.json");
            var jsonContent = "{\"text\": \"Hello\"}";
            File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = filePath,
                ["Encoding"] = "utf-8"
            };

            var result = WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

            await Assert.That(result).IsNotNull();
        }

        [Test]
        public async Task WithCustomCulture_LoadsWithSpecifiedCulture()
        {
            var dir = NewDir();

            var filePath = Path.Combine(dir, "data.json");
            var jsonContent = "{\"value\": \"123,45\"}";
            File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = filePath,
                ["Culture"] = new CultureInfo("pt-BR")
            };

            var result = WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

            await Assert.That(result).IsNotNull();
        }
    }
}