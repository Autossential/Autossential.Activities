using System.Activities;

namespace Autossential.Activities
{
    public sealed class ReplaceTokens : CodeActivity<string>
    {
        [RequiredArgument]
        public InArgument<string> Content { get; set; }

        [RequiredArgument]
        public InArgument<Dictionary<string, object>> Dictionary { get; set; }

        [RequiredArgument]
        public InArgument<string> Pattern { get; set; } = "{{0}}";

        [RequiredArgument]
        public InArgument<char> Placeholder { get; set; } = '0';

        protected override string Execute(CodeActivityContext context)
        {
            var content = Content.Get(context);
            if (content == null)
                return content;

            var value = Dictionary.Get(context);
            var pattern = Pattern.Get(context);
            var placeholder = Placeholder.Get(context);

            var token = pattern.Split(placeholder);
            var prefix = token[0];
            var suffix = token[1];

            foreach (var item in value)
            {
                if (!content.Contains(prefix, StringComparison.CurrentCulture))
                    break;

                content = content.Replace(prefix + item.Key + suffix, item.Value?.ToString());
            }

            return content;
        }
    }
}
