using System.Activities;
using System.Activities.Presentation.Model;
using System.Windows;
using System.Windows.Forms;

namespace Autossential.Shared.Activities.Design.Controls
{
    // Interaction logic for FolderPickerControl.xaml
    public partial class FolderPickerControl
    {
        public FolderPickerControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ModelItemProperty = DependencyProperty.Register("ModelItem", typeof(ModelItem), typeof(FolderPickerControl));

        public ModelItem ModelItem
        {
            get { return GetValue(ModelItemProperty) as ModelItem; }
            set { SetValue(ModelItemProperty, value); }
        }

        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(FolderPickerControl));

        public string PropertyName
        {
            get { return GetValue(PropertyNameProperty) as string; }
            set { SetValue(PropertyNameProperty, value); }
        }

        public bool ShowNewFolderButton { get; set; } = true;

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.ShowNewFolderButton = ShowNewFolderButton;
                var dialog = fbd.ShowDialog();
                if (dialog == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                {
                    ModelItem.Properties[PropertyName].SetValue(InArgument<string>.FromValue(fbd.SelectedPath));
                }
            }
        }
    }
}