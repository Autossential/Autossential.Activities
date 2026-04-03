using System;
using System.Activities;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Autossential.Activities.Models;
using Xunit;

namespace Autossential.Activities.Tests.Activities
{
    public class LoadDataFileTests
    {
        [Fact]
        public void Invoke_WithValidJsonFile_LoadsAndParsesJson()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            try
            {
                var filePath = Path.Combine(dir, "data.json");
                var jsonContent = "{\"name\": \"Alice\", \"age\": 30}";
                File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

                var inputs = new Dictionary<string, object>
                {
                    ["FilePath"] = filePath
                };

                var result = (DataNode)WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

                Assert.NotNull(result);
                Assert.Equal("Alice", result["name"].AsString());
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Fact]
        public void Invoke_WithValidYamlFile_LoadsAndParsesYaml()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            try
            {
                var filePath = Path.Combine(dir, "data.yaml");
                var yamlContent = "name: Bob\nage: 25";
                File.WriteAllText(filePath, yamlContent, Encoding.UTF8);

                var inputs = new Dictionary<string, object>
                {
                    ["FilePath"] = filePath
                };

                var result = (DataNode)WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

                Assert.NotNull(result);
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Fact]
        public void Invoke_WithNonExistentFile_ThrowsFileNotFoundException()
        {
            var inputs = new Dictionary<string, object>
            {
                ["FilePath"] = "/nonexistent/path/file.json"
            };

            Assert.Throws<FileNotFoundException>(() => WorkflowInvoker.Invoke(new LoadDataFile(), inputs));
        }

        [Fact]
        public void Invoke_WithCustomEncoding_LoadsWithSpecifiedEncoding()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            try
            {
                var filePath = Path.Combine(dir, "data.json");
                var jsonContent = "{\"text\": \"Hello\"}";
                File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

                var inputs = new Dictionary<string, object>
                {
                    ["FilePath"] = filePath,
                    ["Encoding"] = "utf-8"
                };

                var result = (DataNode)WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

                Assert.NotNull(result);
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        [Fact]
        public void Invoke_WithCustomCulture_LoadsWithSpecifiedCulture()
        {
            var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            try
            {
                var filePath = Path.Combine(dir, "data.json");
                var jsonContent = "{\"value\": \"123,45\"}";
                File.WriteAllText(filePath, jsonContent, Encoding.UTF8);

                var inputs = new Dictionary<string, object>
                {
                    ["FilePath"] = filePath,
                    ["Culture"] = new CultureInfo("pt-BR")
                };

                var result = (DataNode)WorkflowInvoker.Invoke(new LoadDataFile(), inputs);

                Assert.NotNull(result);
            }
            finally
            {
                try { Directory.Delete(dir, true); } catch { }
            }
        }
    }
}
