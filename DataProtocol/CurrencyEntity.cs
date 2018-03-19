using System.ComponentModel.DataAnnotations;
// ReSharper disable InconsistentNaming

namespace DataProtocol {
    public enum CurrencyType {
        USD, EUR, GBP, ILS, BTC, JMD
    }

    public class Quotes {

        public double USDEUR { get; set; }

        public double USDGBP { get; set; }

        public double USDILS { get; set; }

        public double USDBTC { get; set; }

        public double USDJMD { get; set; }
    }

    public class LiveCurrencyEntity {

        public string success { get; set; }

        public string terms { get; set; }

        public string privacy { get; set; }

        public int timestamp { get; set; }

        public string source { get; set; }

        public Quotes quotes { get; set; }

        /// <summary>
        /// get the respective value from the qoutes
        /// </summary>
        /// <param name="type">currency code</param>
        /// <returns>value for the chosen currency from qoute</returns>
        public double GetValue(string type)
        {
            switch (type) {
                case "USD": return 1;
                case "BTC": return quotes.USDBTC;
                case "EUR": return quotes.USDEUR;
                case "ILS": return quotes.USDILS;
                case "JMD": return quotes.USDJMD;
                case "GBP": return quotes.USDGBP;
                default: return -1; // Error occured
            }
        }
    }

    public class HistoricalCurrencyEntity : LiveCurrencyEntity {

        public string historical { get; set; }

        [Key]
        public string date { get; set; }

    }
}
