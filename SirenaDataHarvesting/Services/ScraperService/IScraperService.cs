using OpenQA.Selenium;

namespace SirenaDataHarvesting.Services.ScraperService
{
    public interface IScraperService
    {
        Task<IReadOnlyCollection<IWebElement>> ScrapeProductsAsync(IWebDriver driver);
    }
}
