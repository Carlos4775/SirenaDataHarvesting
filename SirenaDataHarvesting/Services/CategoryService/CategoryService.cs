using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Category>> GetAsync() =>
            await _genericRepository.AsQueryable().ToListAsync();
    }
}
