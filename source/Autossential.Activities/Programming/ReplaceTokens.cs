using Autossential.Activities.Properties;
using System.Activities;
using System.Collections.Generic;

namespace Autossential.Activities
{
    public sealed class ReplaceTokens : CodeActivity<string>
    {
        public InArgument<string> Content { get; set; }
        public InArgument<Dictionary<string, object>> InputDictionary { get; set; }
        public InArgument<string> Pattern { get; set; } = "{{0}}";
        public InArgument<char> Placeholder { get; set; } = '0';

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);

            if (Content == null) metadata.AddValidationError(ResourcesFn.Validation_ValueErrorFormat(nameof(Content)));
            if (InputDictionary == null) metadata.AddValidationError(ResourcesFn.Validation_ValueErrorFormat(nameof(InputDictionary)));
            if (Pattern == null) metadata.AddValidationError(ResourcesFn.Validation_ValueErrorFormat(nameof(Pattern)));
            if (Placeholder == null) metadata.AddValidationError(ResourcesFn.Validation_ValueErrorFormat(nameof(Placeholder)));
        }

        protected override string Execute(CodeActivityContext context)
        {
            var content = Content.Get(context);
            if (content == null)
                return content;

            var value = InputDictionary.Get(context);
            var pattern = Pattern.Get(context);
            var placeholder = Placeholder.Get(context);

            var token = pattern.Split(placeholder);
            var prefix = token[0];
            var suffix = token[1];

            foreach (var item in value)
            {
                if (content.IndexOf(prefix) == -1)
                    break;

                content = content.Replace(prefix + item.Key + suffix, item.Value?.ToString());
            }

            return content;
        }
    }
}