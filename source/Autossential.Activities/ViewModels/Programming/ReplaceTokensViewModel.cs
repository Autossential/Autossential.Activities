using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Collections.Generic;

namespace Autossential.Activities.ViewModels.Programming
{
    public class ReplaceTokensViewModel : BaseViewModel
    {
        public ReplaceTokensViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignInArgument<string> Content { get; set; }
        public DesignInArgument<Dictionary<string, object>> InputDictionary { get; set; }
        public DesignInArgument<string> Pattern { get; set; }
        public DesignInArgument<char> Placeholder { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Content.IsRequired = true;
            Content.IsPrincipal = true;
            Content.Category = Resources.Input_Category;
            Content.DisplayName = Resources.ReplaceTokens_Content_DisplayName;
            Content.Tooltip = Resources.ReplaceTokens_Content_Description;
            Content.OrderIndex = orderIndex++;

            InputDictionary.IsRequired = true;
            InputDictionary.IsPrincipal = true;
            InputDictionary.Category = Resources.Input_Category;
            InputDictionary.DisplayName = Resources.ReplaceTokens_InputDictionary_DisplayName;
            InputDictionary.Tooltip = Resources.ReplaceTokens_InputDictionary_Description;
            InputDictionary.OrderIndex = orderIndex++;

            Pattern.IsRequired = false;
            Pattern.IsPrincipal = false;
            Pattern.Category = Resources.Options_Category;
            Pattern.DisplayName = Resources.ReplaceTokens_Pattern_DisplayName;
            Pattern.Tooltip = Resources.ReplaceTokens_Pattern_Description;
            Pattern.OrderIndex = orderIndex++;

            Placeholder.IsRequired = false;
            Placeholder.IsPrincipal = false;
            Placeholder.Category = Resources.Options_Category;
            Placeholder.DisplayName = Resources.ReplaceTokens_Placeholder_DisplayName;
            Placeholder.Tooltip = Resources.ReplaceTokens_Placeholder_Description;
            Placeholder.OrderIndex = orderIndex++;
        }
    }
}
