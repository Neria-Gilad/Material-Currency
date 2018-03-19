using System.Collections.Generic;
using System.Threading.Tasks;
using DataProtocol;

namespace Project.Models
{
    public class LiveModel
    {
        /// <summary>
        /// gets live info about the selected currencies
        /// </summary>
        /// <returns>list of the currencies with their live info</returns>
        public async Task<List<CurrencyFields>> GetLiveListAsync()
        {
            return await new BL.Bl().GetLiveListAsync();
        }
    }
}
