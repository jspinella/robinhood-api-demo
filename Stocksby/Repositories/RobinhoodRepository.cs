using Flurl.Http;
using StackExchange.Redis;
using Stocksby.Models.Robinhood;
using System.Text.Json;

namespace Stocksby.Repositories
{
    public static class RobinhoodRepository
    {
        public static async Task<(string token, DateTime expirationDate)> GetBearerToken(string username, string password)
        {
            var response = await "https://api.robinhood.com/oauth2/token/"
                .WithHeaders(new
                {
                    Accept = "*/*",
                    User_Agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0",
                    X_Robinhood_API_Version = "1.431.4",
                    Content_Type = "application/json",
                })
                .PostJsonAsync(new
                {
                    username,
                    password,
                    grant_type = "password",
                    client_id = "c82SH0WZOsabOXGP2sxqcj34FxkvfnWRZBKlBjFS",
                    device_token = "03896f95-d612-4dca-b11f-f75275a18e21"
                })
                .ReceiveJson<TokenResponse>();

            var expirationDate = DateTime.Now.AddSeconds(response.expires_in);

            return (response.access_token, expirationDate);
        }

        // volatile / non-volatile
        public static async Task<QuoteResponse> GetQuote(string token, string ticker)
        {
            var conn = ConnectionMultiplexer.Connect("localhost");
            var database = conn.GetDatabase();

            var cache = await database.StringGetAsync($"quote:{ticker}");

            if (cache.IsNull)
            {
                var response = await $"https://api.robinhood.com/quotes/{ticker.ToUpper()}/"
                    .WithHeaders(new
                    {
                        Accept = "*/*",
                        User_Agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0",
                        X_Robinhood_API_Version = "1.431.4",
                        Content_Type = "application/json",
                    })
                   .WithOAuthBearerToken(token)
                   .GetStringAsync();

                await database.StringSetAsync($"quote:{ticker}", response, new TimeSpan(0, 0, 5));

                return JsonSerializer.Deserialize<QuoteResponse>(response);
            }

            Console.WriteLine("Using Redis Cache!");
            return JsonSerializer.Deserialize<QuoteResponse>(cache.ToString());
        }

        // semi-volatile
        public static async Task<EarningsResponse> GetEarnings(string token, string instrumentId)
        {
            var conn = ConnectionMultiplexer.Connect("localhost");
            var database = conn.GetDatabase();

            var cache = await database.StringGetAsync($"earnings:{instrumentId}");

            if (cache.IsNull)
            {
                var url = $"https://api.robinhood.com/marketdata/earnings/?instrument=/instruments/{instrumentId}/";
                var response = await url
                    .WithHeaders(new
                    {
                        Accept = "*/*",
                        User_Agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:67.0) Gecko/20100101 Firefox/67.0",
                        X_Robinhood_API_Version = "1.431.4",
                        Content_Type = "application/json",
                    })
                    .WithOAuthBearerToken(token)
                    .GetStringAsync();

                await database.StringSetAsync($"earnings:{instrumentId}", response, new TimeSpan(1,0,0,0));

                return JsonSerializer.Deserialize<EarningsResponse>(response);
            }

            Console.WriteLine("Using Redis Cache!");
            return JsonSerializer.Deserialize<EarningsResponse>(cache.ToString());
        }
    }
}
