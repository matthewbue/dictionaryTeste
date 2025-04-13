using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Domain.Interfaces
{
    public interface IWordRepository
    {
        Task<Word> GetWordByNameAsync(string word);
        Task<IEnumerable<Word>> GetWordsAsync(string search, int limit, int page);
        Task AddToFavoritesAsync(string wordId);
        Task RemoveFromFavoritesAsync(string wordId);
        Task AddWordsAsync(List<Word> words);
        Task<int> GetTotalWordsAsync();
        Task<List<Word>> GetWordsAsync(int page, int limit);
    }
}
