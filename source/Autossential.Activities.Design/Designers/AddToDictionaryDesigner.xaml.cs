using Autossential.Shared.Activities.Design.Features;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for AddToDictionaryDesigner.xaml
    public partial class AddToDictionaryDesigner
    {
        public AddToDictionaryDesigner()
        {
            InitializeComponent();
        }

        protected override void OnModelItemChanged(object newItem)
        {
            base.OnModelItemChanged(newItem);
            DesignerFeatures.AddSupportForUpdatingTypeArgument(ModelItem, new[] { "ArgumentType Key", "ArgumentType Value" });
        }
    }
}