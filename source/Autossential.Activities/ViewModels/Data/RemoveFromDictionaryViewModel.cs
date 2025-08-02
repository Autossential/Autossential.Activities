using Autossential.Activities.Properties;
using System.Activities;
using System.Activities.DesignViewModels;
using System.Collections.Generic;

namespace Autossential.Activities.ViewModels.Data
{
    public class RemoveFromDictionaryViewModel : DesignPropertiesViewModel
    {
        public RemoveFromDictionaryViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument Dictionary { get; set; }

        public DesignInArgument Key { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;
            Dictionary.IsPrincipal = true;
            Dictionary.IsRequired = true;
            Dictionary.Category = Resources.Input_Category;
            Dictionary.DisplayName = Resources.RemoveFromDictionary_InputDictionary_DisplayName;
            Dictionary.Placeholder = Resources.RemoveFromDictionary_InputDictionary_Description;
            Dictionary.OrderIndex = orderIndex++;

            Key.IsPrincipal = true;
            Key.IsRequired = true;
            Key.Category = Resources.Input_Category;
            Key.DisplayName = Resources.RemoveFromDictionary_Key_DisplayName;
            Key.Placeholder = Resources.RemoveFromDictionary_Key_Description;
            Key.OrderIndex = orderIndex++;
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
            
            if (!Dictionary.HasValue)
                return;

            var argType = Dictionary.Value.ArgumentType;
            if (argType.IsGenericType && argType.GenericTypeArguments.Length == 2 && typeof(IDictionary<,>).MakeGenericType(argType.GenericTypeArguments).IsAssignableFrom(argType))
            {
                var types = argType.GenericTypeArguments;
                Dictionary.DisplayName += $"<{types[0].Name},{types[1].Name}>";
                Key.DisplayName += $"<{types[0].Name}>";
            }
        }
    }
}
