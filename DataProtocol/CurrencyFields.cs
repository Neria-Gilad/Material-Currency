using System.Collections.Generic;

namespace DataProtocol
{
    /// <summary>
    /// defines a currency with its value
    /// </summary>
    public class CurrencyFields
    {
     
        public static Dictionary<string, string> SymbolsDictionary = new Dictionary<string, string>()
        {
            {"USD", "$"},
            {"EUR", "€"},
            {"GBP", "£"},
            {"ILS", "₪"},
            {"BTC", "Ƀ"},
            {"JMD", "J$"}
        };
        public static Dictionary<string, string> NameDictionary = new Dictionary<string, string>()
        {
            {"USD", "US Dollar"},
            {"EUR", "Euro"},
            {"GBP", "British Pound"},
            {"ILS", "IL New Shekel"},
            {"BTC", "Bitcoin"},
            {"JMD", "Jamaican Dollar"}
        };
        public string Code { get; set; }
        public double Value { get; set; }
    }
}
