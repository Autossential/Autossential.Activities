using Autossential.Activities.Base;
using Autossential.Activities.Extensions;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class ReplaceTokensViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<string> Content { get; set; }
        public DesignInArgument<Dictionary<string, object>> Dictionary { get; set; }
        public DesignInArgument<string> Pattern { get; set; }
        public DesignInArgument<char> Placeholder { get; set; }
        public DesignProperty<bool> CaseSensitive { get; set; }
        public DesignOutArgument<string> Result { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            int orderIndex = 0;

            Content.IsPrincipal = true;
            Content.OrderIndex = orderIndex++;

            Dictionary.IsPrincipal = true;
            Dictionary.OrderIndex = orderIndex++;

            AddWidget(CaseSensitive, ViewModelWidgetType.Toggle);
            CaseSensitive.OrderIndex = orderIndex++;
        }
    }
}
