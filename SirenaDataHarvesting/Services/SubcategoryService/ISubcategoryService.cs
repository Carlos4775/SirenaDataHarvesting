using SirenaDataHarvesting.Models;

namespace SirenaDataHarvesting.Services.SubcategoryService
{
    public interface ISubcategoryService
    {
        Task<List<Subcategory>> GetAsync();
    }
}
