using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Project.Converters
{
    /// <summary>
    /// converts a time span to an SVG with its first letter
    /// </summary>
    internal class CharToVectorConverter : IValueConverter
    {
        private static readonly Dictionary<string, string> VectorsDictionary = new Dictionary<string, string>()
        {
            {"Year", "M2 -492h178l240 625q79 214 98 309h8q13 -51 54.5 -174.5t271.5 -759.5h178l-471 1248q-70 185 -163.5 262.5t-229.5 77.5q-76 0 -150 -17v-133q55 12 123 12q171 0 244 -192l61 -156z"},
            {"Month", "M1573 1116v-713q0 -131 -56 -196.5t-174 -65.5q-155 0 -229 89t-74 274v612h-166v-713q0 -131 -56 -196.5t-175 -65.5q-156 0 -228.5 93.5t-72.5 306.5v575h-166v-1096h135l27 150h8q47 -80 132.5 -125t191.5 -45q257 0 336 186h8q49 -86 142 -136t212 -50 q186 0 278.5 95.5t92.5 305.5v715h-166z"},
            {"Week", "M1071 1096l-201 -643q-19 -59 -71 -268h-8q-40 175 -70 270l-207 641h-192l-299 -1096h174q106 413 161.5 629t63.5 291h8q11 -57 35.5 -147.5t42.5 -143.5l201 -629h180l196 629q56 172 76 289h8q4 -36 21.5 -111t208.5 -807h172l-303 1096h-197z"}
        };

        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string letter && VectorsDictionary.TryGetValue(letter, out var vectorPath))
                return vectorPath;

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
