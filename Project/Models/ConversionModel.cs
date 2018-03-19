using System.Collections.Generic;
using System.Threading.Tasks;
using BL;
using DataProtocol;

namespace Project.Models {
    public class ConversionModel {

        /// <summary>
        /// convert currencies from the selected currency
        /// </summary>
        /// <param name="from">source currency</param>
        /// <param name="mult">multiplier</param>
        /// <returns>list of converted currencies</returns>
        public async Task<List<CurrencyFields>> ConvertCurrencyListAsync(string from,string mult)
        {
            return await new Bl().ConvertCurrencyListAsync(from,double.Parse(mult));
        }

        
    }
}
