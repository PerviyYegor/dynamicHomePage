using System;
using Newtonsoft.Json;
using System.Net;

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

    public static void PrintWeatherData(WeatherData weatherData)
    {
        Console.WriteLine($"Latitude: {weatherData.latitude}");
        Console.WriteLine($"Longitude: {weatherData.longitude}");
        Console.WriteLine($"Generation Time: {weatherData.generationtime_ms} (UTC{weatherData.utc_offset_seconds / 3600:+#;-#})");
        Console.WriteLine($"Timezone: {weatherData.timezone} ({weatherData.timezoneAbbreviation})");
        Console.WriteLine($"Elevation: {weatherData.elevation} m\n");

        Console.WriteLine("Hourly Data:");
        Console.WriteLine($"{ "Time",20} { "Temperature",15} { "Humidity",15} { "Rainfall",15}");
        for (int i = 0; i < weatherData.hourly.time.Count; i++)
        {
            Console.WriteLine($"{weatherData.hourly.time[i],20} {weatherData.hourly.temperature_2m[i],15:F1} {weatherData.hourly.relativehumidity_2m[i],15:F0}% {weatherData.hourly.rain[i],15:F2} mm");
        }
        Console.WriteLine();

        Console.WriteLine("Daily Data:");
        Console.WriteLine($"Today is {weatherData.daily.time[0]}");
        Console.WriteLine($"and best sunset will at {weatherData.daily.sunset[0]}");
        
    }
}


class Program
{
    public static string apiWeatherURL = "https://api.open-meteo.com/v1/forecast?latitude=50.45&longitude=30.52&hourly=temperature_2m,relativehumidity_2m,rain&daily=sunset&windspeed_unit=ms&forecast_days=1&timezone=auto";
    static void Main(string[] args)
    {
        try
        {
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(apiWeatherURL);
                WeatherData weatherDatas = JsonConvert.DeserializeObject<WeatherData>(json);


                WeatherData.PrintWeatherData(weatherDatas);
            }
        }
        catch (WebException)
        {
            Console.WriteLine($"Parsing went wrong :(");
        }
    }
}
