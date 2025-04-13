using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;

public class HistoryService : IHistoryService
{
    private readonly IHistoryRepository _historyRepository;

    public HistoryService(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    public async Task AddToHistoryAsync(string userId, string word)
    {
        await _historyRepository.AddHistoryAsync(userId, word);
    }

    public async Task<List<History>> GetHistoryAsync(string userId)
    {
        return (List<History>)await _historyRepository.GetHistoryAsync(userId);
    }

    public async Task ClearHistoryAsync(string userId)
    {
        await _historyRepository.ClearHistoryAsync(userId);
    }

    Task<IEnumerable<string>> IHistoryService.GetHistoryAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
