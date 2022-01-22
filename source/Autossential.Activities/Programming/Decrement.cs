using Autossential.Activities.Properties;
using System;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class Decrement : CodeActivity
    {
        [RequiredArgument]
        public InArgument<int> Value { get; set; }

        [RequiredArgument]
        public InOutArgument<int> Variable { get; set; }

        public Decrement()
        {
            Value = new InArgument<int>(1);
        }

        protected override void Execute(CodeActivityContext context)
        {
            var value = Value.Get(context);
            if (value < 1)
                throw new InvalidOperationException(Resources.Decrement_ErrorMsg_MinValue);

            Variable.Set(context, Variable.Get(context) - value);
        }
    }
}