using System.Diagnostics;

namespace Portfolio.Console.WebScraper;

public class FileHandler
{
    public static void OpenFileWithDefaultApp(string fullName)
    {
        var argument = "/open, \"" + fullName + "\"";

        Process.Start("explorer.exe", argument);
    }
}