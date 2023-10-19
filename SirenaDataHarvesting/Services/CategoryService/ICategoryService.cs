using OpenQA.Selenium;
using SirenaDataHarvesting.Models;

namespace SirenaDataHarvesting.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Task<Category?> GetAsync(Category category);
        Task CreateCategoriesAsync(IReadOnlyCollection<IWebElement> categoryElements);
        Task CreateAsync(Category category);
        Task UpdateAsync(Category updatedCategory);
        bool AreCategoriesDifferent(Category existingCategory, Category newCategory);
    }
}
