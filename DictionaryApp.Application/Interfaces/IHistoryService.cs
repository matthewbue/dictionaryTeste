using DictionaryApp.Domain.Entities;

namespace DictionaryApp.Application.Interfaces
{
    public interface IHistoryService
    {
        Task AddToHistoryAsync(string userId, string word);
        Task ClearHistoryAsync(string userId);
        Task<IEnumerable<History>> GetHistoryAsync(string userId);
    }
}