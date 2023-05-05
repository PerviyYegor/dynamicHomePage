using System.IO;

class dynamicHomePage
{
    public static void Main(){
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        WeatherDataFacade.collectWeatherData(50.45, 30.52, $"json/{today}/weather.json",$"humanReadable/{today}/weather.txt");
    }
    private static void writeToFile(string path, string content)
    {
        string directoryPath = Path.GetDirectoryName(path);
        Directory.CreateDirectory(directoryPath);
        File.WriteAllText(path, content);
    }
}
