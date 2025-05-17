using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> GetAllAsync();
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        Task<bool> ExistsAsync(string username);
    }
}
