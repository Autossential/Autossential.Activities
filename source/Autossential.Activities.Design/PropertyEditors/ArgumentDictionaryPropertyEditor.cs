using System.Activities.Presentation;
using System.Activities.Presentation.Converters;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.PropertyEditing;
using System.Windows;

namespace Autossential.Activities.Design.PropertyEditors
{
    public sealed class ArgumentDictionaryPropertyEditor : DialogPropertyValueEditor
    {
        public ArgumentDictionaryPropertyEditor()
        {
            InlineEditorTemplate = PropertyEditorResources.GetDataTemplate("ArgumentDictionaryPropertyEditor");
        }

        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            var ownerActivityConverter = new ModelPropertyEntryToOwnerActivityConverter();
            var activityItem = ownerActivityConverter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null) as ModelItem;
            var editingContext = activityItem.GetEditingContext();
            var parentModelItem = ownerActivityConverter.Convert(propertyValue.ParentProperty, typeof(ModelItem), true, null) as ModelItem;
            var args = parentModelItem.Properties[propertyValue.ParentProperty.PropertyName].Dictionary;

            var options = new DynamicArgumentDesignerOptions
            {
                Title = $"{activityItem.ItemType.Name}.{propertyValue.ParentProperty.DisplayName}"
            };

            using (var modelEditingScope = args.BeginEdit("Editing"))
            {
                if (DynamicArgumentDialog.ShowDialog(activityItem, args, editingContext, activityItem.View, options))
                {
                    modelEditingScope.Complete();
                }
                else
                {
                    modelEditingScope.Revert();
                }
            }
        }
    }
}