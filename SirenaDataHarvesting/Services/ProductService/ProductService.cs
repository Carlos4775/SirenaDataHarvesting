using OpenQA.Selenium;
using SirenaDataHarvesting.Models;
using SirenaDataHarvesting.Services.GenericRepository;
using SirenaDataHarvesting.Utils;

namespace SirenaDataHarvesting.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _genericRepository;

        public ProductService(IGenericRepository<Product> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<Product?> GetAsync(Product product) =>
            await _genericRepository.FindOneAsync(x => x.Name == product.Name && x.Price == product.Price && x.ImageUrl == product.ImageUrl);

        public async Task CreateProductsAsync(IReadOnlyCollection<IWebElement> productElements)
        {
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

                Product? existingProduct = await GetAsync(product);

                if (existingProduct == null)
                {
                    await CreateAsync(product);
                }
                else
                {
                    if (AreProductsDifferent(existingProduct, product))
                    {
                        await UpdateAsync(product);
                    }
                }
            }
        } 

        public async Task CreateAsync(Product product) =>
            await _genericRepository.InsertOneAsync(product);

        public async Task UpdateAsync(Product updatedProduct) =>
            await _genericRepository.ReplaceOneAsync(updatedProduct);

        public bool AreProductsDifferent(Product existingProduct, Product newProduct)
        {
            return existingProduct.Name != newProduct.Name || existingProduct.Price != newProduct.Price || existingProduct.ImageUrl != newProduct.ImageUrl;
        }
    }
}
