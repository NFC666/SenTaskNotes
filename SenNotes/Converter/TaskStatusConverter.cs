using System.Globalization;
using System.Windows.Data;

using TaskStatus = SenNotes.Common.Models.TaskStatus;

namespace SenNotes.Converter
{
    public class TaskStatusConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is TaskStatus status)
            {
                switch (status)
                {
                    case TaskStatus.Easy:
                        return "当前状态：轻松";
                    case TaskStatus.Notice:
                        return "当前状态：值得注意";
                    case TaskStatus.Emergency:
                        return "当前状态：紧急";
                    case TaskStatus.Overdue:
                        return "当前状态：已逾期";
                    default:
                        return "未知状态";
                }

            }
            return "出错！";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}