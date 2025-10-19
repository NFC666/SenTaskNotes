using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SenNotes.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;
            bool invert = parameter?.ToString() == "Invert";
            if (invert)
                boolValue = !boolValue;

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                bool result = visibility == Visibility.Visible;
                bool invert = parameter?.ToString() == "Invert";
                return invert ? !result : result;
            }
            return false;
        }
    }

}