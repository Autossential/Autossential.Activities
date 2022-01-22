using Autossential.Shared.Activities.Design.Features;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for ExtractDataColumnValuesDesigner.xaml
    public partial class ExtractDataColumnValuesDesigner
    {
        public ExtractDataColumnValuesDesigner()
        {
            InitializeComponent();
        }

        protected override void OnModelItemChanged(object newItem)
        {
            base.OnModelItemChanged(newItem);
            DesignerFeatures.AddSupportForUpdatingTypeArgument(ModelItem);
        }
    }
}