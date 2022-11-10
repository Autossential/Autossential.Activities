using System;
using System.Activities;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Presentation.View;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Autossential.Shared.Activities.Design.Controls.Editors
{
    // Interaction logic for ExpressionDialogEditor.xaml
    public partial class ExpressionDialogEditor
    {
        public ModelItem OwnerActivity { get; }
        public PropertyValue PropertyValue { get; }
        public ModelItem Value { get; set; }
        public Type ExpressionType { get; }
        public bool UseLocationExpression { get; }
        public string DisplayProperty { get; }
        public string HintText { get; }
        public string ArgumentLabel { get; set; }

        public ExpressionDialogEditor(ModelItem ownerActivity, PropertyValue propertyValue, bool? useLocationExpression = null)
        {
            OwnerActivity = ownerActivity;
            PropertyValue = propertyValue;

            var prop = ownerActivity.Properties[propertyValue.ParentProperty.PropertyName];
            Value = prop.Value;
            ExpressionType = prop.PropertyType.GetGenericArgumentType();
            UseLocationExpression = useLocationExpression ?? prop.PropertyType.Name.Contains(typeof(OutArgument).Name);
            Title = "Expression Editor";
            InitializeLabel(prop.Name);
            InitializeComponent();
            TextBoxControl.Loaded += TextBoxControl_Loaded;
        }

        private void InitializeLabel(string propertyName)
        {
            var type = ExpressionType == null
                ? PropertyValue.ParentProperty.PropertyType.Name
                : ExpressionType.Name;

            ArgumentLabel = $"{propertyName} ({type})";
        }

        private void TextBoxControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (UseLocationExpression)
            {
                Binding binding = new Binding(nameof(Value))
                {
                    Converter = new ArgumentToExpressionConverter(),
                    ConverterParameter = ArgumentDirection.Out,
                    Mode = BindingMode.TwoWay
                };
                BindingOperations.SetBinding(TextBoxControl, ExpressionTextBox.ExpressionProperty, binding).UpdateTarget();
            }
            TextBoxControl.Focus();
            TextBoxControl.BeginEdit();
            TextBoxControl.Loaded -= TextBoxControl_Loaded;
        }

        protected override void OnWorkflowElementDialogClosed(bool? dialogResult)
        {
            if (dialogResult == true)
                (DesignerView.CommitCommand as RoutedCommand).Execute(null, TextBoxControl);

            base.OnWorkflowElementDialogClosed(dialogResult);
        }
    }
}