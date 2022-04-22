using Stocksby.Models.StocksbyDB;
using Stocksby.Repositories;

namespace Stocksby.Services
{
    internal static class StocksbyService
    {
        // validate ticker and get instrumentId either from MySQL DB or Robinhood API
        public static async Task<string> GetInstrumentId(string token, string ticker)
        {
            var dbResult = await StocksbyRepository.GetInstrument(ticker);

            if (dbResult == null)
            {
                var rhResult = await RobinhoodService.GetQuote(token, ticker);

                if (rhResult == null) throw new Exception("Invalid ticker provided!");

                var instrument = new Instrument
                {
                    InstrumentId = rhResult.instrument_id,
                    Ticker = rhResult.symbol
                };

                // add instrument to MySQL DB
                await StocksbyRepository.InsertInstrument(instrument);

                return instrument.InstrumentId;
            }

            return dbResult.InstrumentId;
        }

        public static async Task<decimal> GetLastTradePrice(string token, string ticker)
        {
            return await RobinhoodService.GetLastTradePrice(token, ticker);
        }

        public static async Task<DateTime?> GetNextEarningsDate(string token, string instrumentId)
        {
            return await RobinhoodService.GetNextEarningsDate(token, instrumentId);
        }

        public static async Task<string> GetBearerToken(string username, string password)
        {
            var (dbToken, tokenExpiry) = await StocksbyRepository.GetBearerToken(username);

            var dbTokenExpired = tokenExpiry <= DateTime.Now;

            if (dbTokenExpired)
            {
                var (newToken, newExpiry) = await RobinhoodRepository.GetBearerToken(username, password);

                // update MySQL DB with new token and expiration date
                await StocksbyRepository.UpdateBearerToken(username, newToken, newExpiry);

                return newToken;
            }

            return dbToken;
        }
    }
}
