using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;

namespace Autossential.Activities.ViewModels.Security.Algorithms
{
    internal class AesAlgorithmEncryptionViewModel : BaseViewModel
    {
        public AesAlgorithmEncryptionViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignProperty<int> Iterations { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            Iterations.OrderIndex = 0;
            Iterations.IsPrincipal = false;
            Iterations.IsRequired = false;
            Iterations.DisplayName = Resources.SymmetricAlgorithmEncryptionBase_Iterations_DisplayName;
            Iterations.Placeholder = Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description;
            Iterations.Tooltip = Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description;
        }
    }
}
