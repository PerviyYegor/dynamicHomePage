using System;
using Newtonsoft.Json;
using System.Net;
using System.IO;

public class HourlyUnits
{
    public string time { get; set; }
    public string temperature_2m { get; set; }
    public string relativehumidity_2m { get; set; }
    public string rain { get; set; }
}

public class Hourly
{
    public List<string> time { get; set; }
    public List<double> temperature_2m { get; set; }
    public List<int> relativehumidity_2m { get; set; }
    public List<double> rain { get; set; }
}

public class DailyUnits
{
    public string time { get; set; }
    public string sunset { get; set; }
}

public class Daily
{
    public List<string> time { get; set; }
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
        weatherInfo += $"Latitude: {weatherData.latitude}\n";
        weatherInfo += $"Longitude: {weatherData.longitude}\n";
        weatherInfo += $"Timezone: {weatherData.timezone} ({weatherData.timezoneAbbreviation})\n";
        weatherInfo += $"Elevation: {weatherData.elevation} m\n\n";

        weatherInfo += "Hourly Data:\n";
        weatherInfo += ($"{"Time",15} {"Temperature",15} {"Humidity",15} {"Rainfall",15}\n");
        for (int i = 0; i < weatherData.hourly.time.Count; i++)
        {
            weatherInfo += ($"{weatherData.hourly.time[i].Substring(weatherData.hourly.time[i].Length-5),15} {weatherData.hourly.temperature_2m[i],15:F1} {weatherData.hourly.relativehumidity_2m[i],15:F0}% {weatherData.hourly.rain[i],15:F2} mm\n");
        }

        weatherInfo += ("Daily Data:\n");
        weatherInfo += ($"Today is {weatherData.daily.time[0]}\n");
        weatherInfo += ($"and best sunset will at {weatherData.daily.sunset[0]}\n");
        return weatherInfo;
    }
}


class WeatherDataFacade
{
    public static string apiWeatherURL = "https://api.open-meteo.com/v1/forecast?latitude=50.45&longitude=30.52&hourly=temperature_2m,relativehumidity_2m,rain&daily=sunset&windspeed_unit=ms&forecast_days=1&timezone=auto";
    public static bool collectWeatherData(double latitude, double longitude, string jsonPath, string humanReadablePath)
    {
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


            Console.WriteLine($"Is fine! Your data in human readable format collect in {humanReadablePath} and in json format in {jsonPath}");
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
