using Autossential.Shared.Activities.Design.Controls.Editors;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;

namespace Autossential.Shared.Activities.Design
{
    public static class PropertyEditorExtensions
    {
        public static void OpenExpressionDialog(this PropertyValue propertyValue)
        {
            var converter = new ModelPropertyEntryToOwnerActivityConverter();
            var ownerActivity = converter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null) as ModelItem;

            using (var editingScope = ownerActivity.BeginEdit())
            {
                var editor = new ExpressionDialogEditor(ownerActivity, propertyValue);
                if (editor.ShowOkCancel())
                {
                    ownerActivity.Properties[propertyValue.ParentProperty.PropertyName].SetValue(editor.Value);
                    editingScope.Complete();
                }
            }
        }
    }
}