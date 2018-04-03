using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataProtocol;

namespace DAL
{
    public interface IDal
    {
        /// <summary>
        /// update the database of currency info
        /// </summary>
        /// <param name="start">beggining of timespan to update</param>
        /// <param name="finish">last of the dates to update</param>
        /// <param name="force">false will start from last available date. true will download them all</param>
        /// <returns></returns>
        Task UpdateDataBaseAsync(DateTime start, DateTime finish, bool force = false);

        /// <summary>
        /// get currency info a number of days back
        /// </summary>
        /// <param name="days">number of days to go back</param>
        /// <returns>list of currency info in the requested range</returns>
        Task<List<HistoricalCurrencyEntity>> GetTimeSpanCurrency(int days);

        /// <summary>
        /// get info regarding a specific date
        /// </summary>
        /// <param name="date">date to return info about</param>
        /// <returns>currency info about chosen date</returns>
       // Task<HistoricalCurrencyEntity> GetCurrencyByDay(DateTime date);//Gilad

        /// <summary>
        /// gets updated, live information about the supported currencies
        /// </summary>
        /// <returns>live information about the currencies</returns>
        Task<LiveCurrencyEntity> GetLiveCurrencyAsync();
    }
}
