using System.Collections.Generic;
using System.Threading.Tasks;
using DataProtocol;

namespace Project.Models
{
    public class GraphModel
    {
        /// <summary>
        /// get all supported currencies
        /// </summary>
        /// <returns>a list of all the currencies</returns>
        public async Task<List<CurrencyFields>> GetAllCurrencies()
        {
            return await new BL.Bl().GetAllCurrenciesAsync();
        }

        /// <summary>
        /// get a list of info wothin the selected timeframe
        /// </summary>
        /// <param name="days">how many days back</param>
        /// <returns>list of the requested currencies</returns>
        public async Task<List<HistoricalCurrencyEntity>> GetRelevanList(int days)
        {
            return await new BL.Bl().GetTimeSpanEntityAsync(days);
        }
    }
}
