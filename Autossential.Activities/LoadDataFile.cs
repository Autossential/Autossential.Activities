using Autossential.Activities.Core;
using Autossential.Activities.Models;
using System.Activities;
using System.Globalization;

namespace Autossential.Activities
{
    public sealed class LoadDataFile : CodeActivity<DataNode>
    {
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        public InArgument<string> Encoding { get; set; }
        public InArgument<CultureInfo> Culture { get; set; }
        protected override DataNode Execute(CodeActivityContext context)
        {
            var filePath = FilePath.Get(context);

            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            var encoding = Encoding.Get(context) ?? System.Text.Encoding.UTF8.BodyName;

            object value;
            var content = File.ReadAllText(filePath, System.Text.Encoding.GetEncoding(encoding));
            if (content.StartsWith("[") || content.StartsWith("{"))
                value = JsonParser.Parse(content);
            else
                value = YamlParser.Parse(content);

            var culture = Culture.Get(context) ?? CultureInfo.InvariantCulture;
            return new DataNode(value, culture);
        }
    }
}