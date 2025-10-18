

using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using SenNotes.Common.Models;

using TaskStatus = SenNotes.Common.Models.TaskStatus;

namespace SenNotes.Converter
{
    public class TaskStatusToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TaskStatus taskModel)
            {
                switch (taskModel)
                {
                    case TaskStatus.Easy:
                        return new SolidColorBrush(Colors.ForestGreen);
                    case TaskStatus.Notice:
                        return new SolidColorBrush(Colors.DarkOrange);
                    case TaskStatus.Emergency:
                        return new SolidColorBrush(Colors.Purple);
                    case TaskStatus.Overdue:
                        return new SolidColorBrush(Colors.DarkRed);
                    default:
                        return new SolidColorBrush(Colors.ForestGreen);
                }
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}