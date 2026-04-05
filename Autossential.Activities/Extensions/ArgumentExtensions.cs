using Microsoft.CSharp.Activities;
using Microsoft.VisualBasic.Activities;
using System.Activities;
using System.Activities.Expressions;

namespace Autossential.Activities.Extensions
{
    internal static class ArgumentExtensions
    {
        extension<T>(Argument arg)
        {
            public string GetExpressionText()
            {
                if (arg.Expression is VisualBasicReference<T> vbRef)
                    return vbRef.ExpressionText;

                if (arg.Expression is CSharpReference<T> csRef)
                    return csRef.ExpressionText;

                if (arg.Expression is Literal<T> lit)
                    return lit.Value?.ToString();

                return "";
            }
        }
    }
}
