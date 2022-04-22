
namespace Stocksby.Models.Robinhood
{
    public class QuoteResponse
    {
        public string ask_price { get; set; }
        public int ask_size { get; set; }
        public string bid_price { get; set; }
        public int bid_size { get; set; }
        public string last_trade_price { get; set; }
        public object last_extended_hours_trade_price { get; set; }
        public string previous_close { get; set; }
        public string adjusted_previous_close { get; set; }
        public string previous_close_date { get; set; }
        public string symbol { get; set; }
        public bool trading_halted { get; set; }
        public bool has_traded { get; set; }
        public string last_trade_price_source { get; set; }
        public DateTime updated_at { get; set; }
        public string instrument { get; set; }
        public string instrument_id { get; set; }
        public string state { get; set; }
    }
}
