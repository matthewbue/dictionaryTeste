namespace DictionaryApp.Application.Interfaces
{
    public interface IHistoryService
    {
        Task AddToHistoryAsync(string userId, string word);
        Task<IEnumerable<string>> GetHistoryAsync(string userId);
        Task ClearHistoryAsync(string userId);
    }
}