using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Autossential.Activities.Design.Controls
{
    // Interaction logic for DockPadControl.xaml
    public partial class DockPadControl
    {
        private Button _currentSelection;

        public DockPadControl()
        {
            InitializeComponent();
        }

        private void DockPad_Loaded(object sender, RoutedEventArgs e)
        {
            Select(mc);
        }

        private void DockPad_Click(object sender, RoutedEventArgs e)
        {
            _currentSelection.Opacity = .5;
            _currentSelection.Background = Brushes.White;
            Select((Button)sender);
        }

        private void Select(Button btn)
        {
            btn.Opacity = .8;
            btn.Background = new SolidColorBrush(Color.FromRgb(200, 225, 250));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(140, 180, 220));
            _currentSelection = btn;
        }
    }
}
