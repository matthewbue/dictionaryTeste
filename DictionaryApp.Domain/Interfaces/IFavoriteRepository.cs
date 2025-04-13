using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Domain.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteAsync(string userId, string wordId);
        Task RemoveFavoriteAsync(string userId, string wordId);
        Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId);
    }
}
