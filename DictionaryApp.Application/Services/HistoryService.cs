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

    public async Task ClearHistoryAsync(string userId)
    {
        await _historyRepository.ClearHistoryAsync(userId);
    }

    public async Task<IEnumerable<History>> GetHistoryAsync(string userId)
    {
        var history = await _historyRepository.GetHistoryAsync(userId);
        var historyDtos = new List<History>();

        foreach (var h in history)
        {
            historyDtos.Add(new History
            {
                Word = h.Word,
                Added = h.Added
            });
        }

        return historyDtos;
    }
}
