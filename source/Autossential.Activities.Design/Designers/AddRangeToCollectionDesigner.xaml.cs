using Autossential.Shared.Activities.Design.Features;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for AddRangeToCollection.xaml
    public partial class AddRangeToCollectionDesigner
    {
        public AddRangeToCollectionDesigner()
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

