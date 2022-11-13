using Autossential.Shared.Activities.Design;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;

namespace Autossential.Activities.Design.PropertyEditors
{
    public class BooleanPropertyEditor : DialogPropertyValueEditor
    {
        public BooleanPropertyEditor()
        {
            InlineEditorTemplate = PropertyEditorResources.GetDataTemplate("BooleanPropertyEditor");
        }

        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            propertyValue.OpenExpressionDialog();
        }
    }
}