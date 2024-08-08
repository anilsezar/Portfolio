using Microsoft.Playwright;
using Portfolio.Console.WebScraper;
using Portfolio.Console.WebScraper.Factories;

var myDomain = "https://www.anil-sezer.com";
var etsy = "https://www.etsy.com";
var sahibinden = "https://www.sahibinden.com";
var collar = "https://www.etsy.com/listing/734183748/kaufman-florentine-pewter-black-and-gold";
var whatismybrowser = "https://www.whatismybrowser.com/";
var ffConfig = "";
var ami = "hIUniquetps://amiunique.org/";

// https://github.com/infosimples/detect-headless

var dirPath = "C:\\Users\\msnan\\Downloads\\delete";

var webPage = new PlaywrightWebPage();

var htmlCode = await webPage.GetPagesHtmlAsync(myDomain);




// await Task.Delay(50000);
//
// var ssPath = Path.Combine(dirPath, "screenshot.png");
// await page.ScreenshotAsync(new PageScreenshotOptions
// {
//     Path = ssPath,
//     FullPage = true
// });
//
// FileHandler.OpenFileWithDefaultApp(ssPath);

