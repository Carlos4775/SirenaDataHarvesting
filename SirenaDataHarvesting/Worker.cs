using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SirenaDataHarvesting.Models;
using SirenaDataHarvesting.Services.CategoryService;
using SirenaDataHarvesting.Services.ProductService;
using SirenaDataHarvesting.Services.ScraperService;
using SirenaDataHarvesting.Utils;

namespace SirenaDataHarvesting
{
    public class Worker: BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IScraperService _scraperService;

        public Worker(
            ILogger<Worker> logger,
            IConfiguration configuration,
            ICategoryService categoryService,
            IProductService productService,
            IScraperService scraperService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _categoryService = categoryService;
            _productService = productService;
            _scraperService = scraperService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int workerDelayInMilliseconds = _configuration.GetValue<int>("DelaySettings:WorkerDelayInMilliseconds")!;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                string baseUrl = _configuration["WebScrapingSettings:TargetUrl"]!;

                IWebDriver driver = new ChromeDriver();

                // Maximize the browser window
                driver.Manage().Window.Maximize();

                // Navigate to the website and find elements that contain the category details
                IReadOnlyCollection<IWebElement> categoryElements = await _scraperService.ScrapeCategoriesAsync(driver);

                await _categoryService.CreateCategoriesAsync(categoryElements);

                List<Category> categories = _categoryService.GetAll();

                foreach (Category category in categories)
                {
                    string categoryName = "category/" + category.Slug;
                    string pageParameters = "?page=1&limit=0&sort=1";
                    string targetUrl = $"{baseUrl}{categoryName}{pageParameters}";

                    // Navigate to the website and find elements that contain the product details
                    IReadOnlyCollection<IWebElement> productElements = await _scraperService.ScrapeProductsAsync(driver, targetUrl);

                    await _productService.CreateProductsAsync(productElements);
                }

                // Close the browser
                driver.Quit();

                await Task.Delay(workerDelayInMilliseconds, stoppingToken);
            }
        }
    }
}
