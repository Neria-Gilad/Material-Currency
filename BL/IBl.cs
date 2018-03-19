using System.Collections.Generic;
using System.Threading.Tasks;
using DataProtocol;

namespace BL
{
    public interface IBl
    {
        /// <summary>
        /// an async function that returns all of the currency types in <see cref="CurrencyFields"/> format
        /// </summary>
        /// <returns>List of CurrencyFields which includes all the supported currencies</returns>
        Task<List<CurrencyFields>> GetAllCurrenciesAsync();

        /// <summary>
        /// converts currencies from one type to another
        /// </summary>
        /// <param name="fromCurrency">code of source currency</param>
        /// <param name="toCurrency">code of destination currency</param>
        /// <returns>returns the ratio SRC/DST </returns>
        Task<double> ConvertAsync(string fromCurrency, string toCurrency);

        /// <summary>
        /// converts a type of currency into all the other types
        /// </summary>
        /// <param name="from">source currency</param>
        /// <param name="mult">multiplier eg how much of the source currency</param>
        /// <returns>List of <see cref="CurrencyFields"/> with values matching the correct ratio</returns>
        Task<List<CurrencyFields>> ConvertCurrencyListAsync(string from, double mult);

        /// <summary>
        /// gets updated current values of all the currency types
        /// </summary>
        /// <returns>an updated list of all the currency values</returns>
        Task<List<CurrencyFields>> GetLiveListAsync();

        /// <summary>
        /// get currencies within a number of days from the current date
        /// </summary>
        /// <param name="days">number of days ago to start</param>
        /// <returns>list of <see cref="HistoricalCurrencyEntity"/> from all the days starting from the requested date</returns>
        Task<List<HistoricalCurrencyEntity>> GetTimeSpanEntityAsync(int days);
    }
}