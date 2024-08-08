using Portfolio.Console.WebScraper.Factories;

namespace Portfolio.Console.WebScraper;

public interface IWebPage
{
    Task<string> GetPagesHtmlAsync(string pageUrl);
}

public class PlaywrightWebPage : IWebPage
{
    public async Task<string> GetPagesHtmlAsync(string pageUrl)
    {
        var browser = await BrowserContextFactory.PrepareAndGetBrowser();

        var page = await browser.NewPageAsync();
        await page.GotoAsync(pageUrl);

        await ImitateHuman.SimulateMouse(page); // todo: page'e extension method olarak ekle bunu?

        await page.EvaluateAsync("() => Object.defineProperty(navigator, 'webdriver', { get: () => false })");

        await ImitateHuman.SimulateMouse(page);

        return await page.ContentAsync();
    }
}