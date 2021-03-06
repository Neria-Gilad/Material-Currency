using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProtocol;
using System.Net;
using DAL.EF_Contexts;
using Newtonsoft.Json;

namespace DAL {
    public class Dal : IDal {
        /// <summary>
        /// for key generation <see cref="GetKey"/>
        /// </summary>
        private readonly Random _rand = new Random();

        /// <summary>
        /// parses a DateTime to a form compatible with CurrencyClayer
        /// </summary>
        /// <param name="date">DateTime to convert</param>
        /// <returns>a string compatible with CurrencyClayer</returns>
        private string ParseDate(DateTime date)
        {
            var month = (date.Month < 10) ? "0" : "";
            month += date.Month.ToString();
            var day = (date.Day < 10) ? "0" : "";
            day += date.Day.ToString();
            return date.Year.ToString() + '-' + month + '-' + day;
        }

        /// <summary>
        /// update the database of currency info
        /// </summary>
        /// <param name="start">beggining of timespan to update</param>
        /// <param name="finish">last of the dates to update</param>
        /// <param name="force">false will start from last available date. true will download them all</param>
        /// <returns></returns>
        public async Task UpdateDataBaseAsync(DateTime start, DateTime finish, bool force = false)
        {
            await Task.Factory.StartNew(() => _updateDataBaseAsync(start, finish, force));
        }

        /// <summary>
        /// another async layer to make sure nothing gets stuck <see cref="UpdateDataBaseAsync"/>
        /// </summary>
        private async Task _updateDataBaseAsync(DateTime start, DateTime finish, bool force)
        {
            using (var ctx = new CurrencyContext()) {

                DateTime checkStart = start.Date;//make sure requested date is not later than the latest downloaded because
                                                 //it is assumed that everything is updated until the latest date available

                if (!force)//if we should only update what is necessary
                    checkStart = DateTime.Parse((from hc in ctx.HistoricalCurrency select hc.date).ToList().LastOrDefault()).AddDays(1);//latest update to the database
                if (start < checkStart) start = checkStart;//make sure everything is updated until latest available

                Task<string>[] taskList = new Task<string>[(finish - start).Days + 1];

                for (int i = 0; start <= finish; i++, start = start.AddDays(1)) { //each day in date range
                    string parsedDate = ParseDate(start);

                    taskList[i] = Task<string>.Factory.StartNew(() => {//real concurrency!
                        string jsonData = "";
                        using (var wc = new WebClient()) {
                            for (int j = 0; j < 4; j++) //try 4 times
                            {
                                try { //we calculate url here so we get a new key each time
                                    string url = @"http://apilayer.net/api/historical?access_key=" + GetKey() + "&date=" +
                                                 parsedDate + @"&currencies=EUR,GBP,ILS,BTC,JMD&format=1";
                                    jsonData = wc.DownloadString(url);
                                    if (jsonData.Contains("success\":true"))//CurrencyClayer's way of saying good
                                        break;
                                }
                                catch (Exception e) {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                        return jsonData;
                    });

                }

                Task.WaitAll(taskList);//wait for all of the threads to finish
                foreach (var item in taskList) {
                    if (item.Result == "") continue; //something went wrong
                    HistoricalCurrencyEntity entity =
                        JsonConvert.DeserializeObject<HistoricalCurrencyEntity>(item.Result);
                    if (!ctx.HistoricalCurrency.Any(e => e.date == entity.date)) { //only add if it's not saved already
                        ctx.HistoricalCurrency.Add(entity);
                    }
                }

                await ctx.SaveChangesAsync();
            }
        }

        /// <summary>
        /// get currency info a number of days back
        /// </summary>
        /// <param name="days">number of days to go back</param>
        /// <returns>list of currency info in the requested range</returns>
        public async Task<List<HistoricalCurrencyEntity>> GetTimeSpanCurrency(int days)
        {
            await UpdateDataBaseAsync(DateTime.Today.AddDays(-days), DateTime.Today); //make sure db is updated
            List<HistoricalCurrencyEntity> ret;

            using (var db = new CurrencyContext()) {
                ret = (from hentinity in db.HistoricalCurrency
                       select hentinity).ToList();
            }
            return ret.GetRange(ret.Count - days, days);//the last days
        }


        /// <summary>
        /// CurrencyClayer free keys are limited to 1000 requests so... ALL THE KEYS!!!
        /// </summary>
        /// <returns>one of the available CurrencyClayer keys</returns>
        private string GetKey()
        {
            return Keys[_rand.Next(0, Keys.Length)];//hopefully statistics come into play
        }

        private static readonly string[] Keys = {
            "7d11aed63b58dddf74d36ff2369ff323",
            "58076a627d929149eedbaa9159423ff6",
            "e0715f57b6713e2c9e7a121847d794db",
            "3e871e8d9e9c99c50ec8b0270b2247f4",
            "de4ef6619c81caa797369e9acb834424",
            "06c5b80cf88d7f836e347af070d3f748",
            "85399f88745dae77c1a154c2acd1bd28",
            "60bc52cca10516c91cc0de83a7489552",
            "6487c30c97dd457e7830280a12eaa9c8",
            "0b6603b48665945e4b4cecc07725bdd1",
            "5d8ad51ff93d8414739353778c923147",
            "86931bd1c51bb0e6c33b550286f56349",
            "b540bfe73f053eed6386379ff9acdddc",
            "9fdc7553b8d6f4dec9eeb826255805b1",
            "a6c9c78e68cf346ccdb0c4c09d46ee4d",
            "36099d61984307f45ddad92bf30fc026",
            "a0cfd6312e362bcf8f464cb1dd29c40c",
            "68265087114652c23e2687a092811529",
            "047269bd70bc4b6e73c96a31c97dbf90"
         };

        /// <summary>
        /// gets updated, live information about the supported currencies
        /// </summary>
        /// <returns>live information about the currencies</returns>
        public async Task<LiveCurrencyEntity> GetLiveCurrencyAsync()
        {
            string url = @"http://apilayer.net/api/live?access_key=" + GetKey() + "&currencies=EUR,GBP,ILS,BTC,JMD&format=1";
            LiveCurrencyEntity entity = new LiveCurrencyEntity();
            using (var wc = new WebClient()) {
                var jsonData = string.Empty;
                for (int i = 0; i < 4; i++) //try 4 times
                    try {
                        jsonData = await wc.DownloadStringTaskAsync(url);
                        entity = JsonConvert.DeserializeObject<LiveCurrencyEntity>(jsonData);
                        if (jsonData.Contains("success\":true"))
                            break;
                    }
                    catch (Exception) {
                        throw new Exception("problem getting live information...");
                    }
            }
            return entity;
        }

    }

}

