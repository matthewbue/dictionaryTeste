using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Domain.Interfaces
{
    public interface IHistoryRepository
    {
        Task AddHistoryAsync(Word word);
        Task<IEnumerable<History>> GetHistoryAsync(string userId);
        Task AddHistoryAsync(string userId, string word);
        Task ClearHistoryAsync(string userId);
    }
}
