using Autossential.Shared.Activities.Design.Features;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for RemoveFromDictionary.xaml
    public partial class RemoveFromDictionaryDesigner
    {
        public RemoveFromDictionaryDesigner()
        {
            InitializeComponent();
        }
        protected override void OnModelItemChanged(object newItem)
        {
            DesignerFeatures.AddSupportForUpdatingTypeArgument(ModelItem, new[] { "ArgumentType Key", "ArgumentType Value" });
        }
    }
}
