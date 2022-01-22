using Microsoft.CSharp.Activities;
using Microsoft.VisualBasic.Activities;
using System.Activities;
using System.Activities.Presentation.Expressions;
using System.Activities.Presentation.Model;

namespace Autossential.Shared
{
    public sealed class ExpressionServiceLanguage
    {
        private static string GetLanguage(ModelItem modelItem)
        {
            return ExpressionActivityEditor.GetExpressionActivityEditor(modelItem.Root.GetCurrentValue());
        }

        public static bool IsCSharpEnv(ModelItem modelItem)
        {
            return GetLanguage(modelItem) == "C#";
        }

        public static CodeActivity<T> CreateExpression<T>(ModelItem modelItem, string expression)
        {
            return CreateExpression<T>(modelItem, expression, expression);
        }

        public static CodeActivity<T> CreateExpression<T>(ModelItem modelItem, string vbExpression, string csExpression)
        {
            if (IsCSharpEnv(modelItem))
                return new CSharpValue<T>(csExpression);

            return new VisualBasicValue<T>(vbExpression);
        }
    }
}