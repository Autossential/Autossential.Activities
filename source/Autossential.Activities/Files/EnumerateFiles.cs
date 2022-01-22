using Autossential.Activities.Properties;
using Autossential.Shared;
using System.Activities;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Autossential.Activities
{
    public sealed class EnumerateFiles : CodeActivity<IEnumerable<string>>
    {
        public InArgument Path { get; set; }
        public InArgument SearchPattern { get; set; }
        public SearchOption SearchOption { get; set; }

        public FileAttributes Exclusions { get; set; } = FileAttributes.Hidden
                                                        | FileAttributes.System
                                                        | FileAttributes.Temporary
                                                        | FileAttributes.Device
                                                        | FileAttributes.Offline;


        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(Result, nameof(Result), true);

            if (Path == null)
            {
                metadata.AddRuntimeArgument(Path, typeof(string), nameof(Path), true);
            }
            else if (Path.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(Path, Path.ArgumentType, nameof(Path), true);
            }
            else
            {
                metadata.AddValidationError(Resources.Validation_TypeErrorFormat("string or IEnumerable<string>", nameof(Path)));
            }

            if (SearchPattern == null)
            {
                metadata.AddRuntimeArgument(SearchPattern, typeof(string), nameof(SearchPattern), false);
            }
            else if (SearchPattern.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(SearchPattern, SearchPattern.ArgumentType, nameof(SearchPattern), false);
            }
            else
            {
                metadata.AddValidationError(Resources.Validation_TypeErrorFormat("string or IEnumerable<string>", nameof(SearchPattern)));
            }
        }

        protected override IEnumerable<string> Execute(CodeActivityContext context)
        {
            var directories = Path.GetAsArray<string>(context);
            var patterns = SearchPattern.GetAsArray<string>(context);
            if (patterns.Length == 0)
                patterns = new[] { "*.*" };

            IEnumerable<string> result = new string[] { };
            foreach (var directory in directories)
            {
                foreach (var pattern in patterns)
                {
                    result = result.Union(Directory.EnumerateFiles(directory, pattern, SearchOption));
                }
            }

            if (Exclusions > 0)
                result = result.Where(filePath => (new FileInfo(filePath).Attributes & Exclusions) == 0);

            return result;
        }
    }
}