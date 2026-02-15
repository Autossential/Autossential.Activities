using Autossential.Activities.Base;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class ReplaceTokensViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> Content { get; set; }
        public DesignInArgument<Dictionary<string, object>> Dictionary { get; set; }
        public DesignInArgument<string> Pattern { get; set; }
        public DesignInArgument<char> Placeholder { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Content.IsRequired = true;
            Content.IsPrincipal = true;
            Content.OrderIndex = orderIndex++;

            Dictionary.IsRequired = true;
            Dictionary.IsPrincipal = true;
            Dictionary.OrderIndex = orderIndex++;
        }
    }
}
