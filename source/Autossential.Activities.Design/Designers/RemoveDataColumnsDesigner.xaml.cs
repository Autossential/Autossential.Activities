using Autossential.Shared;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for RemoveDataColumnsDesigner.xaml
    public partial class RemoveDataColumnsDesigner
    {
        public RemoveDataColumnsDesigner()
        {
            InitializeComponent();

            Loaded += RemoveDataColumnsDesigner_Loaded;
        }

        private void RemoveDataColumnsDesigner_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ExpressionServiceLanguage.IsCSharpEnv(ModelItem))
                ColumnsTextBox.HintText = "e.g: new [] {\"Col 1\", \"Col 2\"... } or new [] {0, 1...}";
            else
                ColumnsTextBox.HintText = "e.g: {\"Col 1\", \"Col 2\"...} or {0, 1...}";
        }
    }
}
