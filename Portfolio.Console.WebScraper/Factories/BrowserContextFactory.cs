using Microsoft.Playwright;

namespace Portfolio.Console.WebScraper.Factories;

public static class BrowserContextFactory
{
    public static async Task<IBrowser> PrepareAndGetBrowser()
    {
        var playwright = await Playwright.CreateAsync();
        
        var browserWithoutContext = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
            // FirefoxUserPrefs =
        });

        var browser = (await browserWithoutContext.NewContextAsync(await SetOptions())).Browser;

        if (browser == null)
            throw new Exception();

        return browser;
    }
    
    private static async Task<BrowserNewContextOptions> SetOptions()
    {
        var o = new BrowserNewContextOptions
        {
            ViewportSize = ViewportSize.NoViewport,
            // ViewportSize = SetMonitorSize(),
            Locale = "en-US,en;q=0.5",
            HasTouch = false,
            Offline = false,
            // Permissions = new []{""}, // check this
            // ScreenSize =
            IsMobile = false,
            JavaScriptEnabled = true,
            AcceptDownloads = true,
            ScreenSize = SetPageViewSize()
        };

        return o;
    }

    private static ViewportSize SetMonitorSize()
    {
        return new ViewportSize
        {
            Height = 1080,
            Width = 2560
        };
    }
    
    private static ScreenSize SetPageViewSize()
    {
        return new ScreenSize
        {
            Height = 1080,
            Width = 2560
        };
    }
}