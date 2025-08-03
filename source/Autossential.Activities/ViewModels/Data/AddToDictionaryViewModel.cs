using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Collections.Generic;

namespace Autossential.Activities.ViewModels.Data
{
    internal class AddToDictionaryViewModel : BaseViewModel
    {
        public AddToDictionaryViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInOutArgument Dictionary { get; set; }

        public DesignInArgument Key { get; set; }

        public DesignInArgument Value { get; set; }

        public DesignProperty<bool> UpdateIfExists { get; set; }

        public DesignProperty<bool> AutoInstantiate { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Dictionary.IsRequired = true;
            Dictionary.IsPrincipal = true;
            Dictionary.DisplayName = Resources.AddToDictionary_ReferenceDictionary_DisplayName;
            Dictionary.Category = Resources.Input_Category;
            Dictionary.Placeholder = Resources.AddToDictionary_ReferenceDictionary_Description;
            Dictionary.Tooltip = Resources.AddToDictionary_ReferenceDictionary_Description;
            Dictionary.OrderIndex = orderIndex++;

            Key.IsRequired = true;
            Key.IsPrincipal = true;
            Key.Category = Resources.Input_Category;
            Key.Placeholder = Resources.AddToDictionary_Key_Description;
            Key.Tooltip = Resources.AddToDictionary_Key_Description;
            Key.DisplayName = Resources.AddToDictionary_Key_DisplayName;
            Key.OrderIndex = orderIndex++;

            Value.IsRequired = true;
            Value.IsPrincipal = true;
            Value.Category = Resources.Input_Category;
            Value.Placeholder = Resources.AddToDictionary_Value_Description;
            Value.Tooltip = Resources.AddToDictionary_Value_Description;
            Value.DisplayName = Resources.AddToDictionary_Value_DisplayName;
            Value.OrderIndex = orderIndex++;

            AutoInstantiate.IsRequired = false;
            AutoInstantiate.IsPrincipal = false;
            AutoInstantiate.DisplayName = Resources.AddToDictionary_AutoInstantiate_DisplayName;
            AutoInstantiate.Tooltip = Resources.AddToDictionary_AutoInstantiate_Description;
            AutoInstantiate.Category = Resources.Options_Category;
            AutoInstantiate.OrderIndex = orderIndex++;

            UpdateIfExists.IsRequired = false;
            UpdateIfExists.IsPrincipal = false;
            UpdateIfExists.DisplayName = Resources.AddToDictionary_UpdateIfExists_DisplayName;
            UpdateIfExists.Tooltip = Resources.AddToDictionary_UpdateIfExists_Description;
            UpdateIfExists.Category = Resources.Options_Category;
            UpdateIfExists.OrderIndex = orderIndex++;
        }

        protected override void InitializeRules()
        {
            base.InitializeRules();
            Rule(nameof(Dictionary), DictionaryChanged, true);
        }

        private void DictionaryChanged()
        {
            Dictionary.DisplayName = Resources.AddToDictionary_ReferenceDictionary_DisplayName;
            Key.DisplayName = Resources.AddToDictionary_Key_DisplayName;
            Value.DisplayName = Resources.AddToDictionary_Value_DisplayName;

            if (!Dictionary.HasValue)
                return;

            var argType = Dictionary.Value.ArgumentType;
            if (argType.IsGenericType && argType.GenericTypeArguments.Length == 2 && typeof(IDictionary<,>).MakeGenericType(argType.GenericTypeArguments).IsAssignableFrom(argType))
            {
                var types = argType.GenericTypeArguments;
                Dictionary.DisplayName += $"<{types[0].Name},{types[1].Name}>";
                Key.DisplayName += $"<{types[0].Name}>";
                Value.DisplayName += $"<{types[1].Name}>";
            }
        }
    }
}
