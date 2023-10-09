using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SirenaDataHarvesting.Services.ScraperService
{
    public class ScraperService : IScraperService
    {
        private readonly IConfiguration _configuration;

        public ScraperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IReadOnlyCollection<IWebElement>> ScrapeProductsAsync()
        {
            string targetUrl = _configuration["WebScrapingSettings:TargetUrl"]!;

            using IWebDriver driver = new ChromeDriver();

            // Navigate to the website
            await Task.Run(() => driver.Navigate().GoToUrl(targetUrl));

            // Find elements that contain the product details
            IReadOnlyCollection<IWebElement> productElements = await Task.Run(() => driver.FindElements(By.CssSelector(".item-product")));

            // Close the browser
            driver.Quit();

            return productElements;
        }
    }
}
