using Stocksby.Services;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

while (true)
{
    try
    {
        Console.WriteLine();
        Console.Write("Enter a ticker to begin: ");
        var ticker = Console.ReadLine().ToUpper();
        if (string.IsNullOrWhiteSpace(ticker)) return;

        // get bearer token
        var username = config.GetValue<string>("RobinhoodAPI:Username");
        var password = config.GetValue<string>("RobinhoodAPI:Password");
        var token = await StocksbyService.GetBearerToken(username, password);

        // validate ticker
        var swTicker = new Stopwatch();
        swTicker.Start();
        var instrumentId = await StocksbyService.GetInstrumentId(token, ticker);
        swTicker.Stop();
        Console.WriteLine($"Validated ticker in {swTicker.ElapsedMilliseconds} ms");

        if (string.IsNullOrWhiteSpace(instrumentId)) return;
        var tickerLoop = true;

        while (tickerLoop)
        {
            Console.WriteLine();
            Console.WriteLine($"Please choose an option for {ticker}: ");
            Console.WriteLine("1 - Get last trade price");
            Console.WriteLine("2 - Get next earnings date");
            Console.WriteLine("3 - Enter a new ticker");
            var option = int.Parse(Console.ReadLine());
            var swData = new Stopwatch();

            switch (option)
            {
                case 1:
                    swData.Start();
                    var lastTradePrice = await StocksbyService.GetLastTradePrice(token, ticker);
                    swData.Stop();
                    Console.WriteLine($"Last trade price: {Math.Round(lastTradePrice, 2)} - {swData.ElapsedMilliseconds} ms");
                    break;
                case 2:
                    swData.Start();
                    var nextEarningsDate = await StocksbyService.GetNextEarningsDate(token, instrumentId);
                    swData.Stop();
                    Console.WriteLine($"Next earnings date: {(nextEarningsDate != null ? $"{Convert.ToDateTime(nextEarningsDate):D}  - {swData.ElapsedMilliseconds} ms" : $"Unknown - {swData.ElapsedMilliseconds} ms")}");
                    break;
                case 3:
                    tickerLoop = false;
                    break;
                default:
                    return;
            }
        }         
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}