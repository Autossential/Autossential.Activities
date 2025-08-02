using Autossential.Activities.Properties;
using Autossential.Core.Extensions;
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
        public InArgument DirectoryPath { get; set; }
        public InArgument SearchPattern { get; set; }
        public SearchOption SearchOption { get; set; }

        public FileAttributes Exclusions { get; set; } = FileAttributes.Hidden
                                                        | FileAttributes.System
                                                        | FileAttributes.Temporary
                                                        | FileAttributes.Device
                                                        | FileAttributes.Offline;

        public InArgument<bool> FullPathMode { get; set; }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            metadata.AddRuntimeArgument(Result, nameof(Result), true);

            if (DirectoryPath == null)
            {
                metadata.AddRuntimeArgument(DirectoryPath, typeof(string), nameof(DirectoryPath), true);
            }
            else if (DirectoryPath.IsArgumentTypeAnyCompatible<string, IEnumerable<string>>())
            {
                metadata.AddRuntimeArgument(DirectoryPath, DirectoryPath.ArgumentType, nameof(DirectoryPath), true);
            }
            else
            {
                metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("string or IEnumerable<string>", nameof(DirectoryPath)));
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
                metadata.AddValidationError(ResourcesFn.Validation_TypeErrorFormat("string or IEnumerable<string>", nameof(SearchPattern)));
            }
        }

        protected override IEnumerable<string> Execute(CodeActivityContext context)
        {
            var directories = DirectoryPath.GetAsHashSet<string>(context);
            var patterns = SearchPattern.GetAsHashSet<string>(context);

            if (patterns.Count == 0)
                patterns = new HashSet<string>(["*"]);

            var fullPathMode = FullPathMode.Get(context);

            IEnumerable<string> result = [];
            foreach (var directory in directories)
            {
                foreach (var pattern in patterns)
                {
                    var files = fullPathMode
                        ? Directory.EnumerateFiles(directory, "*", SearchOption).Where(path => path.IsMatch(pattern))
                        : Directory.EnumerateFiles(directory, "*", SearchOption).Where(path => Path.GetFileName(path).IsMatch(pattern));

                    result = result.Union(files);
                }
            }

            if (Exclusions > 0)
                result = result.Where(filePath => (new FileInfo(filePath).Attributes & Exclusions) == 0);

            return result;
        }
    }
}