using System.Activities;
using System.Activities.Expressions;
using System.Activities.Presentation.Model;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for CultureScopeDesigner.xaml
    public partial class CultureScopeDesigner
    {
        public CultureScopeDesigner()
        {
            InitializeComponent();
        }

        private void ExpressionTextBox_EditorLostLogicalFocus(object sender, RoutedEventArgs e)
        {
            DisplayCultureName();
        }

        private void DisplayCultureName()
        {
            if (ModelItem.Properties[nameof(CultureScope.CultureName)].Value?.GetCurrentValue() is InArgument<string> arg)
            {
                try
                {
                    if (arg.Expression is Literal<string>)
                    {
                        var info = CultureInfo.CreateSpecificCulture(arg.Expression.ToString());
                        CultureLabel.Content = info.EnglishName;
                    }
                    else
                    {
                        CultureLabel.Content = "Dynamic";
                    }
                }
                catch
                {
                    CultureLabel.Content = "Invalid Content";
                }
            }
            else
            {
                CultureLabel.Content = Thread.CurrentThread.CurrentCulture.EnglishName;
            }
        }

        private void CultureLabel_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayCultureName();
        }
    }
}