using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Interfaces;
using DictionaryApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryApp.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<IEnumerable<FavoriteDto>> GetFavoriteWordsAsync(string userId)
        {
            var favorites = await _favoriteRepository.GetFavoritesAsync(userId);
            var favoriteDtos = new List<FavoriteDto>();

            foreach (var f in favorites)
            {
                favoriteDtos.Add(new FavoriteDto
                {
                    WordId = f.WordId,
                    Added = f.Added
                });
            }

            return favoriteDtos;
        }
    }
}
