using System.Globalization;
using System.Windows.Data;

namespace SenNotes.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime == DateTime.MinValue)
                {
                    return "无限期";
                }

                return dateTime.ToString("yyyy-MM-dd hh:mm");
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                str = str.Trim();

                if (str == "无限期")
                    return DateTime.MinValue;

                // 尝试多种格式解析
                if (DateTime.TryParseExact(str, "yyyy-MM-dd HH:mm", culture, DateTimeStyles.None, out DateTime result) ||
                    DateTime.TryParseExact(str, "yyyy-MM-dd hh:mm", culture, DateTimeStyles.None, out result) ||
                    DateTime.TryParse(str, culture, DateTimeStyles.None, out result))
                {
                    return result;
                }
            }

            return DateTime.MinValue;
        }


    }
}