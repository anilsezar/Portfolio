using Microsoft.Playwright;

namespace Portfolio.Console.WebScraper;

public class ImitateHuman
{
    private static readonly Random Rand = new();

    public static async Task Delay()
    {
        var delay = Rand.Next(1000, 5000);
        await Task.Delay(delay);
    }
    
    
    public static async Task SimulateMouse(IPage page)
    {
        await page.Mouse.MoveAsync(Rand.Next(0, 1000), Rand.Next(0, 800));
        await page.Keyboard.TypeAsync("Some text", new KeyboardTypeOptions { Delay = Rand.Next(100, 200) });

        for (var i = 0; i < 10; i++)
        {
            await page.Mouse.WheelAsync(0, Rand.Next(50, 200));
            await Task.Delay(Rand.Next(1000, 3000));
        }
    }
}