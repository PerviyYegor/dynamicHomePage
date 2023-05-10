using System.IO;

class dynamicHomePage
{
    public static void Main()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        Console.WriteLine("Starting collect new API data");
        if (WeatherDataFacade.collectWeatherData(50.45, 30.52, $"json/{today}/weather.json", $"humanReadable/{today}/weather.txt") &&
        ExchangeRateFacade.collectWeatherData($"json/{today}/exchangeRate.json", $"humanReadable/{today}/exchangeRate.txt") &&
        quoteData.getQuote($"json/{today}/quote.json", $"humanReadable/{today}/quote.txt"))
            Console.WriteLine("Parse of data went okay! Congratulations! See you tomorrow.");
        else
            Console.WriteLine("Parse of data went wrong! Hope tomorrow attempt will be better");
    }
}
