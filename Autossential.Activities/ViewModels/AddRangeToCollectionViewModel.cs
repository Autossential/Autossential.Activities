using Autossential.Activities.Base;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class AddRangeToCollectionViewModel<T>(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<ICollection<T>> Collection { get; set; }
        public DesignInArgument<IEnumerable<T>> Items { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            Collection.IsPrincipal = true;
            Items.IsPrincipal = true;
        }
    }
}
