using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<Favorite> GetByIdAsync(string wordId);
        Task<Favorite> GetByUserIdAndWordNameAsync(string userId, string wordName);
        Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId);
        Task RemoveAsync(Favorite favorite);
        Task AddAsync(Favorite favorite);
    }
}
