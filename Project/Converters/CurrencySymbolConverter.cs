using System;
using System.Globalization;
using System.Windows.Data;
using DataProtocol;

namespace Project.Converters
{
    /// <summary>
    /// convert a currency code to its symbol
    /// </summary>
    internal class CurrencySymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string code && CurrencyFields.SymbolsDictionary.TryGetValue(code, out var symbol))
                return symbol;

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}