using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<User> GetByIdAsync(string userId);
    }
}
