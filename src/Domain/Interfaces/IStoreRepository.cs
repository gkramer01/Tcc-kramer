using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
