using Dapper;
using Dapper.Contrib.Extensions;
using MySqlConnector;
using Stocksby.Models.StocksbyDB;

namespace Stocksby.Repositories
{
    public static class StocksbyRepository
    {
        public static MySqlConnection DatabaseFactory()
        {
            var connectionString = "server=localhost;user=stocksby;database=stocksby;password=12345;";
            return new MySqlConnection(connectionString);
        }

        // get bearer token from DB
        public static async Task<(string, DateTime)> GetBearerToken(string username)
        {
            var sql = @"SELECT token, tokenExpiry FROM users WHERE username = @username";

            using var conn = DatabaseFactory();
            var results = await conn.QueryAsync<(string, DateTime)>(sql, new { username });

            return results.SingleOrDefault();
        }

        // update bearer token and expiration to DB
        public static async Task UpdateBearerToken(string username, string token, DateTime expiry)
        {
            var sql = @"UPDATE users 
                        SET token = @token, tokenExpiry = @expiry 
                        WHERE username = @username";

            using var conn = DatabaseFactory();
            await conn.ExecuteAsync(sql, new { username, token, expiry });
        }

        // get instrument by ticker
        public static async Task<Instrument?> GetInstrument(string ticker)
        {
            var sql = @"SELECT * FROM instruments WHERE ticker = @ticker";

            using var conn = DatabaseFactory();
            var results = await conn.QueryAsync<Instrument>(sql, new { ticker });

            return results.SingleOrDefault();
        }

        // insert new instrument record
        public static async Task InsertInstrument(Instrument instrument)
        {
            using var conn = DatabaseFactory();
            await conn.InsertAsync(instrument);
        }

    }
}
