using Microsoft.EntityFrameworkCore;
using SirenaDataHarvesting.Models;
using SirenaDataHarvesting.Services.GenericRepository;

namespace SirenaDataHarvesting.Services.SubcategoryService
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IGenericRepository<Subcategory> _genericRepository;

        public SubcategoryService(IGenericRepository<Subcategory> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<List<Subcategory>> GetAsync() =>
            await _genericRepository.AsQueryable().ToListAsync();
    }
}
