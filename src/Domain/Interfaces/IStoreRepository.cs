using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IStoreRepository
    {
        Task AddStoreAsync(Store store);
        Task DeleteStoreAsync(Guid id);
        Task<List<Store>> GetAllAsync();
        Task<Store?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(string name);
        Task UpdateStoreAsync(Store store);
        Task<List<Store>> GetByNameAsync(string storeName);
    }
}
