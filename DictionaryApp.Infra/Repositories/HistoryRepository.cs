using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using DictionaryApp.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DictionaryApp.Infra.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly AppDbContext _context;

        public HistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddHistoryAsync(Word word)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<History>> GetHistoryAsync(string userId)
        {
            return await _context.Histories
                .Where(h => h.UserId == userId)
                .ToListAsync();
        }

        Task<IEnumerable<History>> IHistoryRepository.GetHistoryAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
