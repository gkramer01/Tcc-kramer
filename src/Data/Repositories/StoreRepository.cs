using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class StoreRepository(DefaultDbContext context) : IStoreRepository
    {
        public async Task AddStoreAsync(Store store)
        {
            foreach (var brand in store.Brands)
            {
                context.Attach(brand);
            }

            context.Stores.Add(store);
            await context.SaveChangesAsync();
        }

        public async Task DeleteStoreAsync(Guid id)
        {
            var store = await GetByIdAsync(id);
            if (store != null)
            {
                store.UpdatedAt = DateTime.UtcNow;
                store.DeletedAt = DateTime.UtcNow;
                store.IsDeleted = true;

                context.Stores.Update(store);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await context.Stores
                .Include(s => s.Brands)
                .Where(s => s.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Store?> GetByIdAsync(Guid id)
        {
            return await context.Stores.Include(s => s.Brands).FirstOrDefaultAsync(s => s.Id == id && s.IsDeleted == false);
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await context.Stores.AnyAsync(s => s.Name == name && s.IsDeleted == false);
        }

        public Task UpdateStoreAsync(Store store)
        {
            store.UpdatedAt = DateTime.UtcNow;
            context.Stores.Update(store);
            return context.SaveChangesAsync();
        }

        public async Task<List<Store>> GetByNameAsync(string storeName)
        {
            return await context.Stores
                .Include(s => s.Brands)
                .Where(s => s.IsDeleted == false && s.Name.Contains(storeName))
                .OrderBy(s => s.Name)
                .Take(10)
                .ToListAsync();
        }
    }
}
