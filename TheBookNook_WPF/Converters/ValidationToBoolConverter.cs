using System.Globalization;
using System.Windows.Data;

namespace TheBookNook_WPF.Converters
{
    internal class ValidationToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasError = (bool)values[0];
            string text = values[1] as string;
            return !hasError && !string.IsNullOrEmpty(text);


        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
