using System.Windows;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for IterateDesigner.xaml
    public partial class IterateDesigner
    {
        public IterateDesigner()
        {
            InitializeComponent();
        }

        protected override void OnModelItemChanged(object newItem)
        {
            base.OnModelItemChanged(newItem);
            ModelItem.PropertyChanged += ModelItem_PropertyChanged;
            SetIndexLabel();
        }

        private void ModelItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Iterate.Reverse))
                SetIndexLabel();
        }

        private void SetIndexLabel()
        {
            IndexLabel = (bool)ModelItem.Properties[nameof(Iterate.Reverse)].ComputedValue ? "Index (Rev)" : "Index";
        }

        public string IndexLabel
        {
            get
            {
                return GetValue(IndexLabelProperty) as string;
            }
            set
            {
                SetValue(IndexLabelProperty, value);
            }
        }

        public static readonly DependencyProperty IndexLabelProperty = DependencyProperty.Register("IndexLabel", typeof(string), typeof(IterateDesigner));
    }
}