using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using DictionaryApp.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace DictionaryApp.Infra.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly AppDbContext _context;

        public WordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Word> GetWordByNameAsync(string word)
        {
            return await _context.Words.FirstOrDefaultAsync(w => w.WordName == word);
        }

        public async Task<IEnumerable<Word>> GetWordsAsync(string search, int limit, int page)
        {
            return await _context.Words
                .Where(w => w.WordName.Contains(search))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task AddToFavoritesAsync(string wordId)
        {
            var word = await _context.Words.FindAsync(wordId);
            if (word != null)
            {
                word.IsFavorite = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromFavoritesAsync(string wordId)
        {
            var word = await _context.Words.FindAsync(wordId);
            if (word != null)
            {
                word.IsFavorite = false;
                await _context.SaveChangesAsync();
            }
        }

        Task<Word> IWordRepository.GetWordByNameAsync(string word)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Word>> IWordRepository.GetWordsAsync(string search, int limit, int page)
        {
            throw new NotImplementedException();
        }

        public async Task AddWordsAsync(List<Word> words)
        {
            await _context.Words.AddRangeAsync(words);
            await _context.SaveChangesAsync();
        }

        // Método para obter a quantidade total de palavras
        public async Task<int> GetTotalWordsAsync()
        {
            return await _context.Words.CountAsync();
        }

        // Método para obter as palavras com paginação
        public async Task<List<Word>> GetWordsAsync(int page, int limit)
        {
            return await _context.Words
                                 .Skip((page - 1) * limit)
                                 .Take(limit)
                                 .ToListAsync();
        }

        public async Task<Word> GetByIdAsync(string wordId)
        {
            return await _context.Words
                .FirstOrDefaultAsync(w => w.Id == wordId);
        }
    }
}
