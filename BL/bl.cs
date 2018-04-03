using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProtocol;
using DAL;

namespace BL {
    public class Bl : IBl
    {

        /// <summary>
        /// an async function that returns all of the currency types in <see cref="CurrencyFields"/> format
        /// </summary>
        /// <returns>List of CurrencyFields which includes all the supported currencies</returns>
        public async Task<List<CurrencyFields>> GetAllCurrenciesAsync()
        {
            return await Task.Factory.StartNew(() => (
                from cur in Enum.GetNames(typeof(CurrencyType))
                select new CurrencyFields { Code = cur, Value = -1 }).ToList());
        }

        /// <summary>
        /// converts currencies from one type to another
        /// </summary>
        /// <param name="fromCurrency">code of source currency</param>
        /// <param name="toCurrency">code of destination currency</param>
        /// <returns>returns the ratio SRC/DST </returns>
        public async Task<double> ConvertAsync(string fromCurrency, string toCurrency)
        {
            HistoricalCurrencyEntity currentInfo = (await new Dal().GetTimeSpanCurrency(1)).FirstOrDefault();//Gilad - was get by day
            double from = 1 / currentInfo.GetValue(fromCurrency); // 1/(USD/from) = from/USD
            double to = currentInfo.GetValue(toCurrency); //USD/to   
            return from * to; // from/USD * USD/to = from/to
        }

        /// <summary>
        /// converts a type of currency into all the other types
        /// </summary>
        /// <param name="from">source currency</param>
        /// <param name="mult">multiplier eg how much of the source currency</param>
        /// <returns>List of <see cref="CurrencyFields"/> with values matching the correct ratio</returns>
        public async Task<List<CurrencyFields>> ConvertCurrencyListAsync(string from, double mult)
        {
            var ret = new List<CurrencyFields>();
            foreach (var cur in Enum.GetNames(typeof(CurrencyType)).ToList()) {
                ret.Add(new CurrencyFields { Code = cur, Value = (await ConvertAsync(from, cur)) * mult });
            }
            return ret;
        }

        /// <summary>
        /// gets updated current values of all the currency types
        /// </summary>
        /// <returns>an updated list of all the currency values</returns>
        public async Task<List<CurrencyFields>> GetLiveListAsync()
        {
            LiveCurrencyEntity res = await new Dal().GetLiveCurrencyAsync();
            return await Task.Factory.StartNew(
                () => (
                from cur in Enum.GetNames(typeof(CurrencyType))
                select new CurrencyFields { Code = cur, Value = 1/res.GetValue(cur) }).ToList()
                );
        }

        /// <summary>
        /// get currencies within a number of days from the current date
        /// </summary>
        /// <param name="days">number of days ago to start</param>
        /// <returns>list of <see cref="HistoricalCurrencyEntity"/> from all the days starting from the requested date</returns>
        public async Task<List<HistoricalCurrencyEntity>> GetTimeSpanEntityAsync(int days)
        {
            return (await new Dal().GetTimeSpanCurrency(days)).ToList();
        }
    }
}
