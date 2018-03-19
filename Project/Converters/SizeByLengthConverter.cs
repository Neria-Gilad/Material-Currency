using System;
using System.Globalization;
using System.Windows.Data;

namespace Project.Converters
{
    /// <summary>
    /// other methods to resize font are ugly
    /// </summary>
    internal class SizeByLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var len = (value as string)?.Length ?? 0;
            if (len <= 5)
                return 34;
            if (len <= 9)
                return 20;
            if (len <= 12)
                return 15;
            if (len <= 16)
                return 13;
            if (len <= 20)
                return 10;
            return 7;

            /*
             *  less then 5 letters [base size] is 34
             *
             *  1,000,000,000.00
             *
             *  9    is 20
             *  12   is 15
             *  16   is 
             */

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
