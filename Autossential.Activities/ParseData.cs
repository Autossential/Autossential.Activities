using Autossential.Activities.Core;
using Autossential.Activities.Models;
using System.Activities;
using System.Globalization;
using System.Text;

namespace Autossential.Activities
{
    public sealed class ParseData : CodeActivity<DataNode>
    {
        [RequiredArgument]
        public InArgument<string> Content { get; set; }
        public InArgument<CultureInfo> Culture { get; set; }

        protected override DataNode Execute(CodeActivityContext context)
        {
            var content = Content.Get(context);
            if (string.IsNullOrEmpty(content))
                return null;

            object value;
            if (content.StartsWith("[") || content.StartsWith("{"))
                value = JsonParser.Parse(content);
            else
                value = YamlParser.Parse(content);

            var culture = Culture.Get(context) ?? CultureInfo.InvariantCulture;
            return new DataNode(value, culture);
        }
    }
}