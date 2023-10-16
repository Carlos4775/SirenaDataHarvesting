using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SirenaDataHarvesting.Services.ProductService;
using SirenaDataHarvesting.Services.ScraperService;

namespace SirenaDataHarvesting
{
    public class Worker: BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;
        private readonly IScraperService _scraperService;

        public Worker(
            ILogger<Worker> logger,
            IConfiguration configuration,
            IProductService productService,
            IScraperService scraperService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _productService = productService;
            _scraperService = scraperService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int workerDelayInMilliseconds = _configuration.GetValue<int>("DelaySettings:WorkerDelayInMilliseconds")!;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                IWebDriver driver = new ChromeDriver();

                //// Navigate to the website and find elements that contain the product details
                IReadOnlyCollection<IWebElement> productElements = await _scraperService.ScrapeProductsAsync(driver);

                await _productService.CreateProductsAsync(productElements);

                // Close the browser
                driver.Quit();

                await Task.Delay(workerDelayInMilliseconds, stoppingToken);
            }
        }
    }
}
