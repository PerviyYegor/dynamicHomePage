using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;

public class HourlyUnits
{
    public string time { get; set; }
    public string temperature_2m { get; set; }
    public string relativehumidity_2m { get; set; }
    public string cloudcover { get; set; }
}

public class Hourly
{
    public List<string> time { get; set; }
    public List<double> temperature_2m { get; set; }
    public List<int> relativehumidity_2m { get; set; }
    public List<double> cloudcover { get; set; }
}

public class DailyUnits
{
    public string time { get; set; }
    public string sunrise { get; set; }
    public string sunset { get; set; }
}

public class Daily
{
    public List<string> time { get; set; }
    public List<string> sunrise { get; set; }
    public List<string> sunset { get; set; }

}

public class WeatherData
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double generationtime_ms { get; set; }
    public int utc_offset_seconds { get; set; }
    public string timezone { get; set; }
    public string timezoneAbbreviation { get; set; }
    public double elevation { get; set; }
    public HourlyUnits hourly_units { get; set; }
    public Hourly hourly { get; set; }
    public DailyUnits daily_units { get; set; }
    public Daily daily { get; set; }

    public static string GetWeatherDataString(WeatherData weatherData)
    {
        string weatherInfo = "";
        weatherInfo += "City: Kyiv\n";
        weatherInfo += $"Latitude: {weatherData.latitude}\n";
        weatherInfo += $"Longitude: {weatherData.longitude}\n\n";
        weatherInfo += ("Daily Data:\n");
        weatherInfo += ($"Today is {weatherData.daily.time[0]}\n");
        weatherInfo += ($"Best sunrise was at {weatherData.daily.sunrise[0].Substring(weatherData.daily.sunrise[0].Length - 5)}\n");
        weatherInfo += ($"and best sunset will at {weatherData.daily.sunset[0].Substring(weatherData.daily.sunset[0].Length - 5)}\n\n");

        weatherInfo += "Hourly Data:\n";
        weatherInfo += ($"{"Time",8} {"Temperature",12} {"Humidity",12} {"CloudCover",12}\n");
        for (int i = 0; i < weatherData.hourly.time.Count; i++)
        {
            weatherInfo += ($"{weatherData.hourly.time[i].Substring(weatherData.hourly.time[i].Length - 5),8} {weatherData.hourly.temperature_2m[i],12:F1} {weatherData.hourly.relativehumidity_2m[i],12:F0}% {weatherData.hourly.cloudcover[i],12:F2}%\n");
        }

        return weatherInfo;
    }

}


class WeatherDataFacade
{
    public static bool collectWeatherData(double latitude, double longitude, string jsonPath, string humanReadablePath)
    {
        string apiWeatherURL = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=temperature_2m,relativehumidity_2m,cloudcover&daily=sunrise,sunset&windspeed_unit=ms&forecast_days=1&timezone=auto";
        try
        {
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(apiWeatherURL);
                WeatherData weatherDatas = JsonConvert.DeserializeObject<WeatherData>(json);
                string weatherInfo = WeatherData.GetWeatherDataString(weatherDatas);


                WeatherDataFacade.writeToFile(jsonPath, json);
                WeatherDataFacade.writeToFile(humanReadablePath, weatherInfo);
            }


            Console.WriteLine($"Is fine! Your data about today weather in human readable format collect in {humanReadablePath} and in json format in {jsonPath}");
            return true;
        }
        catch (WebException)
        {
            Console.WriteLine($"Weather parsing went wrong :(");
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
