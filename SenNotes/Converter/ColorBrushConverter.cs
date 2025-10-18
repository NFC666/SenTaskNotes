
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using Color = System.Windows.Media.Color;

namespace SenNotes.Converter
{
    public class ColorBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Color color)
            {
                return new SolidColorBrush(color);
            }
            return new SolidColorBrush(Colors.ForestGreen);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}