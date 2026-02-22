using Autossential.Activities.Properties;
using System.Activities;
using System.Text;

namespace Autossential.Activities
{
    public sealed class RandomString : CodeActivity<string>
    {
        [RequiredArgument]
        public InArgument<string> Format { get; set; } = "Aa*0*Aa?";
        public InArgument<string> Custom { get; set; }

        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";
        private static readonly ThreadLocal<Random> _rng = new(() => new Random());

        protected override string Execute(CodeActivityContext context)
        {
            var format = Format.Get(context);
            if (string.IsNullOrEmpty(format))
                throw new NullReferenceException();

            var custom = Custom.Get(context) ?? "";

            var all = Lowercase + Uppercase + Digits + custom;
            var sb = new StringBuilder();
            var escape = false;
            var rng = _rng.Value;

            foreach (var placeholder in format)
            {
                if (escape)
                {
                    sb.Append(placeholder);
                    escape = false;
                    continue;
                }

                switch (placeholder)
                {
                    case '\\':
                        escape = true;
                        break;
                    case 'a':
                        sb.Append(Lowercase[rng.Next(0, Lowercase.Length)]);
                        break;
                    case 'A':
                        sb.Append(Uppercase[rng.Next(0, Uppercase.Length)]);
                        break;
                    case '0':
                        sb.Append(Digits[rng.Next(0, Digits.Length)]);
                        break;
                    case '*':
                        sb.Append(all[rng.Next(0, all.Length)]);
                        break;
                    case '?' when !string.IsNullOrEmpty(custom):
                        sb.Append(custom[rng.Next(0, custom.Length)]);
                        break;
                    default:
                        sb.Append(placeholder);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
