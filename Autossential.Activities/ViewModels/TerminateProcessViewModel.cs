using Autossential.Activities.Base;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels
{
    internal class TerminateProcessViewModel(IDesignServices services) : BaseViewModel(services)
    {
        public DesignInArgument<IReadOnlyList<string>> ProcessNames { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            ProcessNames.IsPrincipal = true;
        }
    }
}
