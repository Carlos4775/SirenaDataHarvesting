using OpenQA.Selenium;
using SirenaDataHarvesting.Models;

namespace SirenaDataHarvesting.Services.ProductService
{
    public interface IProductService
    {
        Task CreateProductsAsync(IReadOnlyCollection<IWebElement> productElements);
        Task CreateAsync(Product product);
        Task<Product?> GetAsync(Product product);
        Task UpdateAsync(Product updatedProduct);
        bool AreProductsDifferent(Product existingProduct, Product newProduct);
    }
}
