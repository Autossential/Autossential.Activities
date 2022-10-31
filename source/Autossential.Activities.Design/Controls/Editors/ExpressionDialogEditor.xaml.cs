using System;
using System.Activities;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Presentation.View;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Autossential.Activities.Design.Controls.Editors
{
    // Interaction logic for ExpressionDialogEditor.xaml
    public partial class ExpressionDialogEditor
    {

        public ModelItem OwnerActivity { get; }

        public ModelItem Argument { get; set; }

        public Type ExpressionType { get; }

        public bool UseLocationExpression { get; }

        public string DisplayProperty { get; }

        public string HintText { get; }

        public string ArgumentLabel { get; set; }

        public ExpressionDialogEditor(
          ModelItem ownerActivity,
          ModelItem argument,
          Type expressionType,
          bool useLocationExpression,
          string displayProperty = null,
          string hintText = null)
        {
            this.OwnerActivity = ownerActivity;
            this.Argument = argument;
            this.Context = ownerActivity.GetEditingContext();
            this.ExpressionType = expressionType;
            this.UseLocationExpression = useLocationExpression;
            this.DisplayProperty = displayProperty;
            this.HintText = hintText;
            this.Title = "Expression Editor";
            this.InitializeArgumentLabel(displayProperty, expressionType, useLocationExpression);
            this.InitializeComponent();
            this.ExpressionTextBoxId.Loaded += new RoutedEventHandler(this.ExpressionEditorLoaded);
        }

        private void InitializeArgumentLabel(
          string displayName,
          Type expressionType,
          bool useLocationExpression)
        {
            this.ArgumentLabel = displayName + "(";
            this.ArgumentLabel = !useLocationExpression || !(expressionType == (Type)null) ? (useLocationExpression || !(expressionType == (Type)null) ? this.ArgumentLabel + expressionType.Name : this.ArgumentLabel + "InArgument") : this.ArgumentLabel + "OutArgument";
            this.ArgumentLabel += ")";
        }

        private void ExpressionEditorLoaded(object sender, RoutedEventArgs e)
        {
            if (this.UseLocationExpression)
            {
                Binding binding = new Binding("Argument")
                {
                    Converter = (IValueConverter)new ArgumentToExpressionConverter(),
                    ConverterParameter = ArgumentDirection.Out,
                    Mode = BindingMode.TwoWay
                };
                BindingOperations.SetBinding((DependencyObject)this.ExpressionTextBoxId, ExpressionTextBox.ExpressionProperty, (BindingBase)binding).UpdateTarget();
            }
            this.ExpressionTextBoxId.Focus();
            this.ExpressionTextBoxId.BeginEdit();
            this.ExpressionTextBoxId.Loaded -= new RoutedEventHandler(this.ExpressionEditorLoaded);
        }

        protected override void OnWorkflowElementDialogClosed(bool? dialogResult)
        {
            if (dialogResult.HasValue && dialogResult.Value)
                (DesignerView.CommitCommand as RoutedCommand).Execute((object)null, (IInputElement)this.ExpressionTextBoxId);
            base.OnWorkflowElementDialogClosed(dialogResult);
        }
    }
}
