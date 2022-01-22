using System.Activities;
using System.ComponentModel;

namespace Autossential.Activities
{
    public sealed class IsTrue : CodeActivity<bool>
    {
        [RequiredArgument]
        [DisplayName("Expression")]
        public InArgument<bool> Value { get; set; }

        protected override bool Execute(CodeActivityContext context)
        {
            return Value.Get(context);
        }
    }
}
