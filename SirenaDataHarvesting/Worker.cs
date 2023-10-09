using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SirenaDataHarvesting.Models;
using SirenaDataHarvesting.Services.ProductService;
using SirenaDataHarvesting.Utils;

namespace SirenaDataHarvesting
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IProductService productService)
        {
            _logger = logger;
            _configuration = configuration;
            _productService = productService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int workerDelayInMilliseconds = _configuration.GetValue<int>("DelaySettings:WorkerDelayInMilliseconds")!;

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                IWebDriver driver = new ChromeDriver();

                string targetUrl = _configuration["WebScrapingSettings:TargetUrl"]!;

                // Navigate to the website
                await Task.Run(() => driver.Navigate().GoToUrl(targetUrl), stoppingToken);

                // Find elements that contain the product details
                IReadOnlyCollection<IWebElement> productElements = await Task.Run(() => driver.FindElements(By.CssSelector(".item-product")));

                foreach (IWebElement productElement in productElements)
                {
                    string? name = productElement.FindElement(By.ClassName("item-product-title")).FindElement(By.TagName("a")).Text;
                    string? priceString = productElement.FindElement(By.ClassName("item-product-price")).FindElement(By.TagName("strong")).Text;
                    string? imageUrl = ImageUrlUtility.ExtractImageUrlFromStyleAttribute(productElement.FindElement(By.ClassName("item-product-image")).GetAttribute("style"));

                    priceString = priceString.Replace("$", ""); // Eliminar el símbolo de dólar

                    decimal price = decimal.Parse(priceString);

                    Product product = new()
                    {
                        Name = name,
                        Price = price,
                        ImageUrl = imageUrl
                    };

                    Product? existingProduct = await _productService.GetAsync(product);

                    if (existingProduct == null)
                    {
                        await _productService.CreateAsync(product);
                    }
                    else
                    {
                        if (_productService.AreProductsDifferent(existingProduct, product))
                        {
                            await _productService.UpdateAsync(product);
                        }
                    }
                }

                // Close the browser
                driver.Quit();

                await Task.Delay(workerDelayInMilliseconds, stoppingToken);
            }
        }
    }
}
