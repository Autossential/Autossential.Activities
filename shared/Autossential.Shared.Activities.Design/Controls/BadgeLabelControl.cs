using System.Windows;
using System.Windows.Controls;

namespace Autossential.Shared.Activities.Design.Controls
{
    public partial class BadgeLabelControl : UserControl
    {
        public BadgeLabelControl()
        {
            InitializeComponent();
        }

        public string Badge
        {
            get { return (string)GetValue(BadgeProperty); }
            set { SetValue(BadgeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Badge.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BadgeProperty =
            DependencyProperty.Register("Badge", typeof(string), typeof(BadgeLabelControl), new PropertyMetadata(null));


        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(BadgeLabelControl), new PropertyMetadata(null));
    }
}
