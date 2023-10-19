using HtmlAgilityPack;
using OpenQA.Selenium;
using SirenaDataHarvesting.Models;
using SirenaDataHarvesting.Services.GenericRepository;

namespace SirenaDataHarvesting.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _genericRepository;

        public CategoryService(IGenericRepository<Category> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public List<Category> GetAll() =>
            _genericRepository.AsQueryable().ToList();

        public async Task<Category?> GetAsync(Category category) =>
            await _genericRepository.FindOneAsync(x => x.Name == category.Name);

        public async Task CreateCategoriesAsync(IReadOnlyCollection<IWebElement> categoryElements)
        {
            foreach (IWebElement categoryElement in categoryElements)
            {
                string? name = categoryElement.FindElement(By.TagName("a")).GetAttribute("innerText");

                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                List<Category>? subcategories = new();

                string categoryElementHtml = categoryElement.GetAttribute("outerHTML");

                var doc = new HtmlDocument();
                doc.LoadHtml(categoryElementHtml);

                // Busca el <ul> con la clase "uk-nav-sub" y que está oculto
                var ulElement = doc.DocumentNode.SelectSingleNode("//ul[contains(@class, 'uk-dropdown-nav')]");

                if (ulElement != null)
                {
                    // Encuentra todos los elementos <a> dentro del <ul>
                    var aElements = ulElement.SelectNodes("./li[not(ul)]/a");

                    if (aElements != null)
                    {
                        foreach (var aElement in aElements)
                        {
                            if (aElement.InnerText == "Ver todos")
                            {
                                continue;
                            }

                            Category subcategory = new()
                            {
                                Name = aElement.InnerText
                            };

                            subcategories.Add(subcategory);
                        }
                    }
                }

                Category category = new()
                {
                    Name = name,
                    Subcategories = subcategories
                };

                Category? existingCategory = await GetAsync(category);

                if (existingCategory == null)
                {
                    await CreateAsync(category);
                }
                else
                {
                    if (AreCategoriesDifferent(existingCategory, category))
                    {
                        await UpdateAsync(category);
                    }
                }
            }
        }

        public async Task CreateAsync(Category category) =>
            await _genericRepository.InsertOneAsync(category);

        public async Task UpdateAsync(Category updatedCategory) =>
            await _genericRepository.ReplaceOneAsync(updatedCategory);

        public bool AreCategoriesDifferent(Category existingCategory, Category newCategory)
        {
            return existingCategory.Name != newCategory.Name;
        }
    }
}
