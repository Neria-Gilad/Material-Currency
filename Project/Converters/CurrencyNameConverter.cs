using System;
using System.Globalization;
using System.Windows.Data;
using DataProtocol;

namespace Project.Converters {
    /// <summary>
    /// convert to upper or lower
    /// </summary>
    internal class CurrencyNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //  NULL is nothing changed
            //  0 is ToUpper
            //  1 is ToLower
            string op = parameter as string;
            if (!(value is string code) || !CurrencyFields.NameDictionary.TryGetValue(code, out var name)) return "";

            switch (op) {
                case "":
                    goto case null; // fallthrough
                case null:
                    return name;
                case "ToUpper":
                    return name.ToUpper();
                case "ToLower":
                    return name.ToLower();
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
