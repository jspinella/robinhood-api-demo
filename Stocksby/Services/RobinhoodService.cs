using Stocksby.Models.Robinhood;
using Stocksby.Repositories;
using System.Globalization;

namespace Stocksby.Services
{
    internal static class RobinhoodService
    {
        // GetBearerToken
        public static async Task<(string token, DateTime tokenExpiry)> GetBearerToken(string username, string password)
        {
            return await RobinhoodRepository.GetBearerToken(username, password);
        }

        // GetLastTradePrice
        public static async Task<decimal> GetLastTradePrice(string token, string ticker)
        {
            var response = (await RobinhoodRepository.GetQuote(token, ticker))?.last_trade_price;
            _ = decimal.TryParse(response, out decimal lastTradePrice);

            return lastTradePrice;
        }

        // GetQuote
        public static async Task<QuoteResponse> GetQuote(string token, string ticker)
        {
            return await RobinhoodRepository.GetQuote(token, ticker);
        }

        // GetNextEarningsDate
        public static async Task<DateTime?> GetNextEarningsDate(string token, string instrumentId)
        {
            var results = (await RobinhoodRepository.GetEarnings(token, instrumentId))?.results;

            if (results == null) return null;

            var earningsDates = results.Where(x => x.call != null).Select(x => x.call.datetime.Date);
            var nextEarningsDate = earningsDates.Where(x => x > DateTime.Now.ToLocalTime()).OrderBy(x => x).FirstOrDefault();

            if (nextEarningsDate == default && earningsDates.Any())
            {
                // estimate next earnings date based on last earnings date (assumes we have the immediately-prior earnings date)
                var lastEarningsDate = earningsDates.OrderByDescending(x => x).FirstOrDefault();
                var estimatedEarningsDate = lastEarningsDate.AddDays(90);

                return estimatedEarningsDate;
            }
            else
            {
                return nextEarningsDate;
            }
        }
    }
}
