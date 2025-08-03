using Autossential.Activities.Properties;
using System.Activities.DesignViewModels;
using System.Activities.ViewModels;

namespace Autossential.Activities.ViewModels.Security.Algorithms
{
    internal class TripleDESAlgorithmEncryptionViewModel : BaseViewModel
    {
        public TripleDESAlgorithmEncryptionViewModel(IDesignServices services) : base(services)
        {
        }

        public DesignProperty<int> Iterations { get; set; }

        [NotMappedProperty]
        public DesignProperty<string> Warning { get; set; }

        protected override void InitializeModel()
        {
            base.InitializeModel();
            PersistValuesChangedDuringInit();

            var orderIndex = 0;

            Iterations.IsPrincipal = false;
            Iterations.IsRequired = false;
            Iterations.DisplayName = Resources.SymmetricAlgorithmEncryptionBase_Iterations_DisplayName;
            Iterations.Placeholder = Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description;
            Iterations.Tooltip = Resources.SymmetricAlgorithmEncryptionBase_Iterations_Description;
            Iterations.OrderIndex = orderIndex++;

            if (IsWidgetSupported(ViewModelWidgetType.TextBlock))
            {
                Warning.IsPrincipal = true;
                Warning.IsVisible = true;
                Warning.Widget = new TextBlockWidget
                {
                    Level = TextBlockWidgetLevel.Warning,
                    Multiline = true
                };
                Warning.Value = "Legacy algorithm. Use TripleDES only for compatibility with legacy applications and data.";
                Warning.OrderIndex = orderIndex++;
            }
        }
    }
}
