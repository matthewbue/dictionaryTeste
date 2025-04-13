using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using DictionaryApp.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DictionaryApp.Infra.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddFavoriteAsync(string userId, string wordId)
        {
            var favorite = new Favorite
            {
                UserId = userId,
                WordId = wordId,
                Added = DateTime.UtcNow
            };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(string userId, string wordId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.WordId == wordId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Favorite>> GetFavoritesAsync(string userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        Task<IEnumerable<Favorite>> IFavoriteRepository.GetFavoritesAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
