using System;
using System.Activities;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.View;
using System.Windows.Threading;

namespace Autossential.Shared.Activities.Design.Features
{
    public static class DesignerFeatures
    {
        internal const string TypeArgumentPropertyName = "ArgumentType";
        internal const string DisplayName = "DisplayName";

        public static void AddSupportForUpdatingTypeArgument(ModelItem modelItem, string[] labels = null)
        {
            var argTypes = modelItem.ItemType.GetGenericArguments();
            var argsLength = argTypes.Length;
            if (argsLength == 0)
                return;

            if (labels != null && labels.Length != argsLength)
                throw new ArgumentException("When labels is set, its length must match the number of generic arguments of the model item type");

            var service = modelItem.GetEditingContext().Services.GetService<AttachedPropertiesService>();
            for (int argIndex = 0; argIndex < argsLength; argIndex++)
            {
                var propertyName = labels != null
                    ? labels[argIndex]
                    : argsLength > 1
                        ? TypeArgumentPropertyName + (argIndex + 1)
                        : TypeArgumentPropertyName;

                ExposeArgumentTypeForUpdate(modelItem, service, argIndex, propertyName);
            }
        }

        private static void ExposeArgumentTypeForUpdate(ModelItem modelItem, AttachedPropertiesService service, int argIndex, string propertyName)
        {
            service.AddProperty(new AttachedProperty<Type>
            {
                Name = propertyName,
                OwnerType = modelItem.ItemType,
                Getter = (mItem) => GetTypeArgument(mItem, argIndex),
                Setter = (mItem, argType) => UpdateTypeArgument(mItem, argType, argIndex),
                IsBrowsable = true
            });            
        }

        private static void UpdateTypeArgument(ModelItem modelItem, Type argType, int argIndex)
        {
            if (modelItem == null)
                return;

            var argTypes = modelItem.ItemType.GetGenericArguments();
            argTypes[argIndex] = argType;

            var editingContext = modelItem.GetEditingContext();
            var itemType = modelItem.ItemType;
            var type = itemType.GetGenericTypeDefinition().MakeGenericType(argTypes);
            var newModelItem = ModelFactory.CreateItem(editingContext, Activator.CreateInstance(type));

            MorphHelper.MorphObject(modelItem, newModelItem);
            MorphHelper.MorphProperties(modelItem, newModelItem);

            if (itemType.IsSubclassOf(typeof(Activity))
                && type.IsSubclassOf(typeof(Activity))
                && string.Equals((string)modelItem.Properties[DisplayName].ComputedValue, GetActivityDefaultName(itemType), StringComparison.Ordinal))
            {
                newModelItem.Properties[DisplayName].SetValue(GetActivityDefaultName(type));
            }

            var service = editingContext.Services.GetService<DesignerView>();
            if (service == null)
                return;

            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() =>
            {
                if (service.RootDesigner != null && ((WorkflowViewElement)service.RootDesigner).ModelItem == modelItem)
                    service.MakeRootDesigner(newModelItem);

                Selection.SelectOnly(editingContext, newModelItem);
            }));
        }

        private static Type GetTypeArgument(ModelItem modelItem, int argIndex)
        {
            return modelItem?.ItemType.GetGenericArguments()[argIndex];
        }

        private static string GetActivityDefaultName(Type activityType)
        {
            return ((Activity)Activator.CreateInstance(activityType)).DisplayName;
        }
    }
}