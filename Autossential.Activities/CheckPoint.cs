using System.Activities;

namespace Autossential.Activities
{
    public sealed class CheckPoint : CodeActivity
    {
        [RequiredArgument]
        public InArgument<bool> Expression { get; set; }

        [RequiredArgument]
        public InArgument<Exception> Exception { get; set; }

        public InArgument<Dictionary<string, string>> Data { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            if (Expression.Get(context))
                return;

            var data = Data.Get(context) ?? [];

            var ex = Exception.Get(context);

            foreach (var (key, value) in data)
                ex.Data[key] = value;
            
            throw ex;
        }
    }
}
