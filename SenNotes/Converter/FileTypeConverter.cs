using System.Globalization;
using System.Windows.Data;

using MaterialDesignThemes.Wpf;

using SenNotes.Common.Models;

namespace SenNotes.Converter
{
    public class FileTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is FileType type)
            {
                switch (type)
                {
                    case FileType.Text:
                        return PackIconKind.Text;
                    case FileType.Doc:
                        return PackIconKind.MicrosoftWord;
                    case FileType.Pdf:
                        return PackIconKind.FilePdfBox;
                    case FileType.Img:
                        return PackIconKind.Image;
                    default:
                        return PackIconKind.Cross;
                }
            }
            return PackIconKind.Cross;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}