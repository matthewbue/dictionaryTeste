using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using DictionaryApp.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryApp.Infra.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly AppDbContext _context;

        public HistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddHistoryAsync(string userId, string word)
        {
            var history = new History
            {
                UserId = userId,
                Word = word,
                Added = DateTime.UtcNow
            };

            await _context.Histories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<History>> GetHistoryAsync(string userId)
        {
            return await _context.Histories
                .Where(h => h.UserId == userId)
                .ToListAsync();
        }

        public async Task ClearHistoryAsync(string userId)
        {
            var userHistory = await _context.Histories
                .Where(h => h.UserId == userId)
                .ToListAsync();

            _context.Histories.RemoveRange(userHistory);
            await _context.SaveChangesAsync();
        }

        public Task AddHistoryAsync(Word word)
        {
            throw new NotImplementedException();
        }
    }
}
