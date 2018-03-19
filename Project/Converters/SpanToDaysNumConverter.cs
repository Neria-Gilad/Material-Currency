using System;
using System.Globalization;
using System.Windows.Data;

namespace Project.Converters {
    /// <summary>
    /// convert timespan to their respective amount in days
    /// </summary>
    internal class SpanToDaysNumConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int param)) return null;
            switch (param) {
                case 7:
                    return "Week";
                case 30:
                    return "Month";
                case 365:
                    return "Year";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value as string) {
                case "Week":
                    return 7;
                case "Month":
                    return 30;
                case "Year":
                    return 365;
                default:
                    return 0;
            }
        }
    }
}
