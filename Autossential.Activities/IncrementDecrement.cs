using Autossential.Activities.Attributes;
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
            var variable = Variable.Get(context);
            Variable.Set(context, (Operation == ArithmeticOperation.Increment)
                ? variable + value
                : variable - value);
        }
    }
}
