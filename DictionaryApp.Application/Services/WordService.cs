using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Interfaces;

namespace DictionaryApp.Application.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public WordService(IWordRepository wordRepository, IHistoryRepository historyRepository, IFavoriteRepository favoriteRepository)
        {
            _wordRepository = wordRepository;
            _historyRepository = historyRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<WordDto> GetWordDetailsAsync(string word)
        {
            var wordDetails = await _wordRepository.GetWordByNameAsync(word);
            if (wordDetails == null) return null;

            await _historyRepository.AddHistoryAsync(wordDetails);

            return new WordDto
            {
                WordName = wordDetails.WordName,
                Definition = wordDetails.Definition
            };
        }

        public async Task<IEnumerable<WordDto>> SearchWordsAsync(string search, int limit, int page)
        {
            var words = await _wordRepository.GetWordsAsync(search, limit, page);
            return words.Select(w => new WordDto
            {
                WordName = w.WordName,
                Definition = w.Definition
            });
        }

        public async Task AddWordToFavoritesAsync(string wordId)
        {
            await _favoriteRepository.AddFavoriteAsync("userId", wordId);  // Coloque o userId real aqui
        }

        public async Task RemoveWordFromFavoritesAsync(string wordId)
        {
            await _favoriteRepository.RemoveFavoriteAsync("userId", wordId);  // Coloque o userId real aqui
        }

        public async Task<IEnumerable<FavoriteDto>> GetFavoriteWordsAsync()
        {
            var favorites = await _favoriteRepository.GetFavoritesAsync("userId"); // Coloque o userId real aqui
            return favorites.Select(f => new FavoriteDto
            {
                WordId = f.WordId,
                Added = f.Added
            });
        }

        public async Task<WordListDto> GetWordsAsync(int page, int limit)
        {
            var totalWords = await _wordRepository.GetTotalWordsAsync();
            var words = await _wordRepository.GetWordsAsync(page, limit);

            var totalPages = (int)Math.Ceiling((double)totalWords / limit);
            var hasNext = page < totalPages;
            var hasPrev = page > 1;

            return new WordListDto
            {
                Results = words,
                TotalDocs = totalWords,
                Page = page,
                TotalPages = totalPages,
                HasNext = hasNext,
                HasPrev = hasPrev
            };
        }
    }
}
