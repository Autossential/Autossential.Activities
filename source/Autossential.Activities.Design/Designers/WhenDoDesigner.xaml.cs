using System.Activities;
using System.Windows;

namespace Autossential.Activities.Design.Designers
{
    // Interaction logic for WhenDoDesigner.xaml
    public partial class WhenDoDesigner
    {
        public WhenDoDesigner()
        {
            InitializeComponent();
            Loaded += WhenDoDesigner_Loaded;
        }

        private void WhenDoDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            WhenObj.AllowedItemType = typeof(Activity<bool>);
        }

        private void AddElse_Click(object sender, RoutedEventArgs e)
        {
            var prop = ModelItem.Properties[nameof(WhenDo.WithElse)];
            var withElse = (bool)prop.ComputedValue;
            prop.SetValue(!withElse);
        }

        protected override void OnModelItemChanged(object newItem)
        {
            base.OnModelItemChanged(newItem);
            ModelItem.PropertyChanged += ModelItem_PropertyChanged;
            UpdateWhenLabel();
        }

        private void ModelItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WhenDo.Inverted))
                UpdateWhenLabel();
        }



        private void UpdateWhenLabel()
        {
            if ((bool)ModelItem.Properties[nameof(WhenDo.Inverted)].Value.GetCurrentValue())
            {
                WhenLabel = "When (Not)";
            }
            else
            {
                WhenLabel = "When";
            }            
        }


        public string WhenLabel
        {
            get
            {
                return GetValue(WhenLabelProperty) as string;
            }
            set
            {
                SetValue(WhenLabelProperty, value);
            }
        }

        public static readonly DependencyProperty WhenLabelProperty = DependencyProperty.Register("WhenLabel", typeof(string), typeof(WhenDoDesigner));
    }
}