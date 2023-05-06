class htmlFormer
{
    public static void getHTML(string weatherInfoPath, string htmlOutputPath)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string inputFilePath = "./humanReadable/2023-05-06/weather.txt";
        string outputFilePath = "docs/index.html";

        // Зчитуємо рядки з вхідного файлу
        string[] lines = File.ReadAllLines(inputFilePath);

        // Створюємо вихідний файл та записуємо до нього HTML-заголовок
        using (StreamWriter sw = new StreamWriter(outputFilePath))
        {
            sw.WriteLine("<html>");
            sw.WriteLine($"<head><title>HomePage</title><link rel=\"stylesheet\" href=\"pageStyles.css\"></head>");
            sw.WriteLine("<body>");

            // Додаємо заголовок
            sw.WriteLine($"<h2>Hello, Friend! Today is {today}</h2>");
            sw.WriteLine("<div>");
            for(int i=0; i<lines.Length;i++){
                sw.WriteLine("<h6>" + lines[i] + "</h6>");
            }
            sw.WriteLine("</div>");
            sw.WriteLine("</body>");
            sw.WriteLine("</html>");
        }

        Console.WriteLine("HTML file created successfully.");
    }
}
