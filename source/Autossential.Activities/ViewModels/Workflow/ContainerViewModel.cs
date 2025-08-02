using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Workflow
{
    public class ContainerViewModel : BaseViewModel
    {
        public ContainerViewModel(IDesignServices services) : base(services) { }
        // Container n�o possui propriedades p�blicas relevantes para exibir no ViewModel.
        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();
        }
    }
}
