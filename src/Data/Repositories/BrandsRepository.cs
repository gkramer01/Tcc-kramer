using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class BrandsRepository(DefaultDbContext context) : IBrandsRepository
    {
        public async Task<List<Brand>> GetByIdsListAsync(List<Guid> ids)
        {
            return await context.Brands.Where(b => ids.Contains(b.Id)).ToListAsync();
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await context.Brands.ToListAsync();
        }
    }
}
