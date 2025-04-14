using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Domain.Interfaces
{
    public interface IWordRepository
    {
        Task<Word> GetWordByNameAsync(string word);
        Task<IEnumerable<Word>> GetWordsAsync(string search, int page, int limit);
        Task AddToFavoritesAsync(string wordId);
        Task RemoveFromFavoritesAsync(string wordId);
        Task AddWordsAsync(List<Word> words);
        Task<int> GetTotalWordsAsync();
        Task<Word> GetByIdAsync(string wordId);
    }
}
