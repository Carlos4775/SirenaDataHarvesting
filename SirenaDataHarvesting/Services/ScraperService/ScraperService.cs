using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace SirenaDataHarvesting.Services.ScraperService
{
    public class ScraperService : IScraperService
    {
        private readonly IConfiguration _configuration;

        public ScraperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IReadOnlyCollection<IWebElement>> ScrapeProductsAsync(IWebDriver driver, string targetUrl)
        {
            // Navigate to the website
            await Task.Run(() => driver.Navigate().GoToUrl(targetUrl));

            Task.Delay(1000).Wait();

            // Find elements that contain the product details
            IReadOnlyCollection<IWebElement> productElements = await Task.Run(() => driver.FindElements(By.CssSelector(".item-product")));

            return productElements;
        }

        public async Task<IReadOnlyCollection<IWebElement>> ScrapeCategoriesAsync(IWebDriver driver)
        {
            string targetUrl = _configuration["WebScrapingSettings:TargetUrl"]!;

            // Navigate to the website
            await Task.Run(() => driver.Navigate().GoToUrl(targetUrl));

            Task.Delay(1000).Wait();

            // Find elements that contain the category details
            IReadOnlyCollection<IWebElement> categoryElements = await Task.Run(() => driver.FindElements(By.CssSelector(".uk-parent")));

            return categoryElements;
        }
    }
}
