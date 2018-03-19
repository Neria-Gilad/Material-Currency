using System;
using System.Globalization;
using System.Windows.Data;
namespace Project.Converters
{
    /// <summary>
    /// convert currency namr to its respective flag from the assets folder
    /// </summary>
    internal class CurrencyFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var code = value as string;
            string a = @"../Assets/" + (string.IsNullOrEmpty(code) ? "NIL" : code) + ".png";
            return a;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
