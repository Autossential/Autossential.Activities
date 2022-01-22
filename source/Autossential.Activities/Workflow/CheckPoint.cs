using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Autossential.Activities
{
    public sealed class CheckPoint : CodeActivity
    {
        [RequiredArgument]
        public InArgument<bool> Expression { get; set; }

        [RequiredArgument]
        public InArgument<Exception> Exception { get; set; }

        [Browsable(true)]
        public Dictionary<string, InArgument> Data { get; } = new Dictionary<string, InArgument>();

        protected override void Execute(CodeActivityContext context)
        {
            if (Expression.Get(context))
                return;

            var ex = Exception.Get(context);

            foreach (var item in Data)
                ex.Data.Add(item.Key, item.Value.Get(context));

            throw ex;
        }
    }
}