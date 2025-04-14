using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;

namespace DictionaryApp.Application.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly JwtTokenService _jwtTokenService;

        public WordService(IWordRepository wordRepository, IHistoryRepository historyRepository, IFavoriteRepository favoriteRepository, JwtTokenService jwtTokenService)
        {
            _wordRepository = wordRepository;
            _historyRepository = historyRepository;
            _favoriteRepository = favoriteRepository;
            _jwtTokenService = jwtTokenService;
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

        public async Task<IEnumerable<FavoriteDto>> GetFavoriteWordsAsync()
        {
            var favorites = await _favoriteRepository.GetFavoritesAsync("userId");
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
                Results = words.Select(w => w.WordName),
                TotalDocs = totalWords,
                Page = page,
                TotalPages = totalPages,
                HasNext = hasNext,
                HasPrev = hasPrev
            };
        }

        public async Task AddWordToFavoritesAsync(string wordId)
    {
            var userId = _jwtTokenService.GetUserIdFromToken();

            var word = await _wordRepository.GetByIdAsync(wordId);
        if (word == null)
        {
            throw new Exception("Palavra não encontrada.");
        }

        var favorite = new Favorite
        {
            UserId = userId,
            WordId = wordId
        };

        await _favoriteRepository.AddAsync(favorite);
    }

    public async Task RemoveWordFromFavoritesAsync(string wordId)
    {
        var userId = "userIdFromToken"; // Suponha que você tenha o userId do token JWT

        var favorite = await _favoriteRepository.GetByUserIdAndWordIdAsync(userId, wordId);
        if (favorite == null)
        {
            throw new Exception("Palavra não está em seus favoritos.");
        }

        await _favoriteRepository.RemoveAsync(favorite);
    }

    // Método para adicionar uma palavra aos favoritos
        public async Task AddToFavoritesAsync(string wordId)
        {
            var userId = "userIdFromToken"; // Substitua com o ID real do usuário (usualmente extraído do token JWT)

            var word = await _wordRepository.GetByIdAsync(wordId);
            if (word == null)
            {
                throw new Exception("Palavra não encontrada.");
            }

            var favorite = new Favorite
            {
                UserId = userId,
                WordId = wordId,
                Added = DateTime.UtcNow
            };

            await _favoriteRepository.AddAsync(favorite);
        }

        // Método para remover uma palavra dos favoritos
        public async Task RemoveFromFavoritesAsync(string wordId)
        {
            var userId = "userIdFromToken"; // Substitua com o ID real do usuário

            var favorite = await _favoriteRepository.GetByUserIdAndWordIdAsync(userId, wordId);
            if (favorite == null)
            {
                throw new Exception("Palavra não está em seus favoritos.");
            }

            await _favoriteRepository.RemoveAsync(favorite);
        }
    }
    

}
