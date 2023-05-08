using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;

public class ExchangeRate
{
        public int r030 { get; set; }
        public string txt { get; set; }
        public double rate { get; set; }
        public string cc { get; set; }
        public string exchangedate { get; set; }

    public static string getExchangeInfo(List<ExchangeRate> exchangeRates)
    {
        string exchangeString = "";
        exchangeString+="The official exchange rate of the hryvnia\n";
        foreach (var rate in exchangeRates)
        {
            exchangeString+=($"{rate.txt} ({rate.cc}): {rate.rate}\n\n");
        }
        return exchangeString;
    }
}

public class ExchangeRateFacade
{
    public static bool collectWeatherData(string jsonPath, string humanReadablePath)
    {
        string apiExchangeUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";

        try
        {
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(apiExchangeUrl);
                List<ExchangeRate> exchangeDatas = JsonConvert.DeserializeObject<List<ExchangeRate>>(json);

                string exchangeInfo = ExchangeRate.getExchangeInfo(exchangeDatas);


                ExchangeRateFacade.writeToFile(jsonPath, json);
                ExchangeRateFacade.writeToFile(humanReadablePath, exchangeInfo);
            }


            Console.WriteLine($"Is fine! Your data about exchange rate in human readable format collect in {humanReadablePath} and in json format in {jsonPath}");
            return true;
        }
        catch (WebException)
        {
            Console.WriteLine($"Exchange data parsing went wrong :(");
            return false;
        }
    }
     private static void writeToFile(string path, string content)
    {
        string directoryPath = Path.GetDirectoryName(path);
        Directory.CreateDirectory(directoryPath);
        File.WriteAllText(path, content);
    }
}
