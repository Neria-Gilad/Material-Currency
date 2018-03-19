using System.Data.Entity;
using DataProtocol;

namespace DAL.EF_Contexts {
    /// <summary>
    /// this is what is saved to the database
    /// the database is called "currencies"
    /// </summary>
    public class CurrencyContext : DbContext {

        public DbSet<HistoricalCurrencyEntity> HistoricalCurrency { get; set; }

        public CurrencyContext() : base("currencies")
        {
        }
    }
}
