using System;
using System.Activities.Expressions;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using Autossential.Shared.Activities.Design;

namespace Autossential.Shared.Activities.Design.Controls
{
    /// <summary>
    /// Interaction logic for BooleanPropertyEditorControl.xaml
    /// </summary>
    public partial class BooleanPropertyEditorControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(BooleanPropertyEditorControl));
        public static readonly DependencyProperty OwnerActivityProperty = DependencyProperty.Register(nameof(OwnerActivity), typeof(ModelItem), typeof(BooleanPropertyEditorControl));
        public static readonly DependencyProperty PropertyHierarchyPathProperty = DependencyProperty.Register(nameof(PropertyHierarchyPath), typeof(string), typeof(BooleanPropertyEditorControl));

        public BooleanPropertyEditorControl()
        {
            Loaded += BooleanPropertyEditorControl_Loaded;
            Unloaded += BooleanPropertyEditorControl_Unloaded;
            InitializeComponent();
        }

        private void BooleanPropertyEditorControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_parentModelItem != null)
                _parentModelItem.PropertyChanged -= _parentModelItem_PropertyChanged;

            Unloaded -= BooleanPropertyEditorControl_Unloaded;
        }

        private void BooleanPropertyEditorControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (OwnerActivity != null)
            {
                _parentModelItem = OwnerActivity.GetParentModelItem(PropertyHierarchyPath);
                Expression = _parentModelItem.Properties[PropertyName]?.Value;
                _parentModelItem.PropertyChanged += _parentModelItem_PropertyChanged;
                UpdateCheckbox();
            }

            Loaded -= BooleanPropertyEditorControl_Loaded;
        }

        private void _parentModelItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != PropertyName)
                return;

            Expression = _parentModelItem.Properties[PropertyName]?.Value;
            UpdateCheckbox();
        }

        public string PropertyHierarchyPath
        {
            get => GetValue(PropertyHierarchyPathProperty) as string;
            set => SetValue(PropertyHierarchyPathProperty, value);
        }

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        public ModelItem OwnerActivity
        {
            get => (ModelItem)GetValue(OwnerActivityProperty);
            set => SetValue(OwnerActivityProperty, value);
        }

        private ModelItem _expression;
        public ModelItem Expression
        {
            get => _expression;
            set
            {
                _expression = _parentModelItem?.Properties[PropertyName]?.SetValue(value);
                OnNotifyPropertyChanged();
            }
        }

        private void OnNotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ModelItem _parentModelItem;

        public event PropertyChangedEventHandler PropertyChanged;

        private void UpdateCheckbox()
        {
            var value = Expression.AsLiteral<bool>();
            if (value.HasValue)
            {
                CheckBoxControl.IsChecked = value.GetValueOrDefault();
                return;
            }

            CheckBoxControl.IsChecked = null;
        }

        private void CheckBoxControl_Checked(object sender, RoutedEventArgs e)
        {
            var value = Expression.AsLiteral<bool>();
            if (value.GetValueOrDefault() && value.HasValue)
                return;

            _parentModelItem.Properties[PropertyName].SetValue(new InArgument<bool>(new Literal<bool>(true)));
        }

        private void CheckBoxControl_Unchecked(object sender, RoutedEventArgs e)
        {
            var value = Expression.AsLiteral<bool>();
            if (!value.GetValueOrDefault() && value.HasValue)
                return;

            _parentModelItem.Properties[PropertyName].SetValue(new InArgument<bool>(new Literal<bool>(false)));
        }

        private void CheckBoxControl_Indeterminate(object sender, RoutedEventArgs e)
        {
            var value = Expression.AsLiteral<bool>();
            if (!value.HasValue)
                return;

            _parentModelItem.Properties[PropertyName].SetValue(null);
        }
    }
}