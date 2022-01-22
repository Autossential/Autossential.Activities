using System;
using System.Globalization;
using System.Windows.Data;

namespace Autossential.Shared.Activities.Design.Converters
{
    public class OptionBooleanToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var intValue = (int)value;
            return intValue == int.Parse(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }
    }
}