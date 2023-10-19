using OpenQA.Selenium;

namespace SirenaDataHarvesting.Services.ScraperService
{
    public interface IScraperService
    {
        Task<IReadOnlyCollection<IWebElement>> ScrapeProductsAsync(IWebDriver driver, string targetUrl);
        Task<IReadOnlyCollection<IWebElement>> ScrapeCategoriesAsync(IWebDriver driver);
    }
}
