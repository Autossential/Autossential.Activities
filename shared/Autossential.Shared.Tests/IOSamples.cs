using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Autossential.Shared.Tests
{
    public static class IOSamples
    {
        private const string TEST_OUTPUT = "unit-tests";

        public static string CreateFolder(string relativeFolderPath)
            => Directory.CreateDirectory(GetTestPath(relativeFolderPath)).FullName;

        public static string GetTestPath(string relativePath = "")
            => Path.GetFullPath(Path.Combine(TEST_OUTPUT, relativePath).Replace("\\", "/"));

        public static string CreateFile(string relativeFilePath)
        {
            var path = GetTestPath(relativeFilePath);
            CreateFolder(Path.GetDirectoryName(path));
            File.WriteAllText(path, "");
            return path;
        }

        public static void CreateFiles(params string[] fileNames)
        {
            foreach (var file in fileNames)
                CreateFile(file);
        }

        public static void CreateFolderAndFiles(string relativeFolderPath, string fileName, params string[] fileNames)
        {
            var folder = CreateFolder(relativeFolderPath);
            CreateFile(Path.Combine(folder, fileName));

            foreach (var name in fileNames)
                CreateFile(Path.Combine(folder, name));
        }

        public static void ClearFolder(string relativeFolderPath = "")
        {
            var path = GetTestPath(relativeFolderPath);
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        public static string ExportSample(string relativeSampleFile)
        {
            var samplesRoot = GetSamplesFolder();
            var source = Path.Combine(samplesRoot, relativeSampleFile);
            if (!File.Exists(source))
                throw new FileNotFoundException(source);

            var target = GetTestPath(relativeSampleFile);
            CreateFolder(Path.GetDirectoryName(target));
            File.Copy(source, target, true);

            return target;
        }

        public static string GetSamplePath(string relativePath)
          => Path.GetFullPath(Path.Combine(GetSamplesFolder(), relativePath).Replace("\\", "/"));

        public static string GetSamplesFolder()
        {
            var path = Environment.CurrentDirectory;
            return Path.Combine(path.Substring(0, path.IndexOf("\\bin\\")), "Samples");
        }
    }
}