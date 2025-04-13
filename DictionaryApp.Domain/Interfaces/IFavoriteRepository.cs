using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<Favorite> GetByIdAsync(string wordId);
        Task<Favorite> GetByUserIdAndWordIdAsync(string userId, string wordId);
        Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId);
        Task RemoveAsync(Favorite favorite);
        Task AddAsync(Favorite favorite);
    }
}
