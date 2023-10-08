using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SirenaDataHarvesting
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                string targetUrl = _configuration["WebScrapingSettings:TargetUrl"]!;
                string csvFilePath = _configuration["WebScrapingSettings:CsvFilePath"]!;

                // Set up ChromeDriver
                IWebDriver driver = new ChromeDriver();

                // Navigate to the website
                driver.Navigate().GoToUrl(targetUrl);

                // Create a list to store the item details
                List<string[]> items = new();

                // Find elements that contain the product details
                IReadOnlyCollection<IWebElement> productElements = driver.FindElements(By.CssSelector(".shelf-item"));

                // Loop through the product elements and extract the desired information
                foreach (IWebElement productElement in productElements)
                {
                    // Extract the name and price of the product
                    string name = productElement.FindElement(By.ClassName("shelf-item__title")).Text;
                    string price = productElement.FindElement(By.ClassName("val")).Text;

                    // Add the item details to the list
                    items.Add(new string[] { name, price });
                }

                // Saving extracted data in CSV file
                using (StreamWriter writer = new(csvFilePath))
                {
                    // Write the CSV header
                    writer.WriteLine("Name,Price");
                    // Write the item details
                    foreach (string[] item in items)
                    {
                        writer.WriteLine(string.Join(",", item));
                    }
                }

                // Close the browser
                driver.Quit();

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
