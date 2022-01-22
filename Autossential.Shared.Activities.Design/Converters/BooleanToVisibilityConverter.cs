using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Autossential.Shared.Activities.Design.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Reverse { get; set; }
        public bool Collapse { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = (bool)value != Reverse;
            return visible ? Visibility.Visible : (Collapse ? Visibility.Collapsed : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Visibility)value;
            return visibility == Visibility.Visible && !Reverse;
        }
    }
}
