using Autossential.Activities.Base;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class UpdateDictionaryViewModel<TKey, TValue>(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<Dictionary<TKey, TValue>> Dictionary { get; set; }
        public DesignInArgument<Dictionary<TKey, TValue>> Entries { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;
            Dictionary.IsPrincipal = true;
            Dictionary.OrderIndex = orderIndex++;
            Entries.IsPrincipal = true;
            Entries.OrderIndex = orderIndex++;

            AddWidget(Dictionary, ViewModelWidgetType.Input);
            AddWidget(Entries, ViewModelWidgetType.Dictionary);
        }
    }
}
