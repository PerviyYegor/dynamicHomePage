using System;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

class Quote
{
    public string quote { get; set; }
    public string author { get; set; }
    public string category { get; set; }

    public static string getQuoteInfo(List<Quote> quos)
    {
        string quot = $"Here is your quote of the day ({DateTime.Now.ToString("yyyy-MM-dd")})\n";
        foreach(var quo in quos ){
            quot += (quo.quote+"\n");
            quot += ($"(c) {quo.author}\n");
        }
        return quot;
    }
}

public class quoteData
{
    public static bool getQuote(string jsonPath, string humanReadablePath)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<dynamicHomePage>()
            .Build();
        string apiKey = config["apiQuoteSet"];


        string api_url = $"https://api.api-ninjas.com/v1/quotes";

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Api-Key", config["apiQuoteSet"]);

        HttpResponseMessage response = client.GetAsync(api_url).Result;
        if (response.IsSuccessStatusCode)
        {
            string json = response.Content.ReadAsStringAsync().Result;

            List<Quote> responseContent = JsonConvert.DeserializeObject<List<Quote>>(json);

            string quotStr = Quote.getQuoteInfo(responseContent);


            quoteData.writeToFile(jsonPath, json);
            quoteData.writeToFile(humanReadablePath, quotStr);

            Console.WriteLine($"Is fine! Your quote in human readable format collect in {humanReadablePath} and in json format in {jsonPath}");
            return true;
        }
        else
        {
            Console.WriteLine("Error: {0} {1}", (int)response.StatusCode, response.ReasonPhrase);
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
