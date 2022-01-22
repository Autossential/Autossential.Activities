using Autossential.Activities.Properties;
using System;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class Increment : CodeActivity
    {
        [RequiredArgument]
        public InArgument<int> Value { get; set; }

        [RequiredArgument]
        public InOutArgument<int> Variable { get; set; }

        public Increment()
        {
            Value = new InArgument<int>(1);
        }

        protected override void Execute(CodeActivityContext context)
        {
            var value = Value.Get(context);
            if (value < 1)
                throw new InvalidOperationException(Resources.Increment_ErrorMsg_MinValue);

            Variable.Set(context, Variable.Get(context) + value);
        }
    }
}