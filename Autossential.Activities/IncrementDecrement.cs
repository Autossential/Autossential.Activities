using Autossential.Activities.Attributes;
using Autossential.Activities.Properties;
using System.Activities;

namespace Autossential.Activities
{
    public sealed class IncrementDecrement : CodeActivity
    {
        public enum ArithmeticOperation
        {
            [LocalizedDisplayName("IncrementDecrement_Operation_Increment")]
            Increment,
            [LocalizedDisplayName("IncrementDecrement_Operation_Decrement")]
            Decrement
        }

        [RequiredArgument]
        public InOutArgument<int> Variable { get; set; }

        [RequiredArgument]
        public InArgument<int> Value { get; set; }

        [RequiredArgument]
        public ArithmeticOperation Operation { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            var value = Value.Get(context);
            if (value < 1)
                throw new InvalidOperationException(Resources.IncrementDecrement_ErrorMsg_MinValueAllowed);

            var variable = Variable.Get(context);
            Variable.Set(context, (Operation == ArithmeticOperation.Increment)
                ? variable + value
                : variable - value);
        }
    }
}
