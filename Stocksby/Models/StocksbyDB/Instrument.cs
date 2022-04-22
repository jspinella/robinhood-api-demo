using Dapper.Contrib.Extensions;

namespace Stocksby.Models.StocksbyDB
{
    [Table("instruments")]
    public class Instrument
    {
        public string InstrumentId { get; set; }
        public string CompanyName { get; set; }
        public string Ticker { get; set; }
        public string CUSIP { get; set; }
    }
}
