using Domain.Entities;
using Domain.Requests.Stores;

namespace Domain.Interfaces
{
    public interface IBrandsRepository
    {
        Task<List<Brand>> GetByIdsListAsync(List<Guid> id);
    }
}
