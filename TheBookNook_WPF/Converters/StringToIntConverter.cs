using System.Globalization;
using System.Windows.Data;

namespace TheBookNook_WPF.Converters
{
    internal class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value as string, out int result))
            {
                return result;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
