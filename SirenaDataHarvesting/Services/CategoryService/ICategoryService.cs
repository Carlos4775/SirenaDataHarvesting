using SirenaDataHarvesting.Models;

namespace SirenaDataHarvesting.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAsync();
    }
}
