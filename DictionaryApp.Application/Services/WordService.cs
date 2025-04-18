﻿using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Interfaces;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;

namespace DictionaryApp.Application.Services
{
    public class WordService : IWordService
    {
        private readonly IWordRepository _wordRepository;
        private readonly ICacheService _cacheService;
        private readonly IHistoryRepository _historyRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public WordService(IWordRepository wordRepository, IHistoryRepository historyRepository, IFavoriteRepository favoriteRepository, IJwtTokenService jwtTokenService
            , ICacheService cacheService)
        {
            _wordRepository = wordRepository;
            _historyRepository = historyRepository;
            _favoriteRepository = favoriteRepository;
            _jwtTokenService = jwtTokenService;
            _cacheService = cacheService;
        }

        public async Task<WordDto> GetWordDetailsAsync(string word)
        {

            string cacheKey = $"word:{word.ToLower()}";

            var cachedWord = await _cacheService.GetAsync<WordDto>(cacheKey);
            if (cachedWord != null)
                return cachedWord;

            var wordDetails = await _wordRepository.GetWordByNameAsync(word);
            if (wordDetails == null) return null;

            var wordDto = new WordDto
            {
                WordName = wordDetails.WordName,
                Definition = wordDetails.Definition
            };

            // Adiciona o resultado ao cache
            await _cacheService.SetAsync(cacheKey, wordDto, TimeSpan.FromMinutes(10));

            return wordDto;
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

        public async Task<WordListDto> GetWordsAsync(string search, int page, int limit)
        {
            var totalWords = await _wordRepository.GetTotalWordsAsync();
            var words = await _wordRepository.GetWordsAsync(search, page, limit);

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

        public async Task AddWordToFavoritesAsync(string wordName)
        {
            var userId = _jwtTokenService.GetUserIdFromToken();

            var word = await _wordRepository.GetWordByNameAsync(wordName);
            if (word == null)
            {
                throw new Exception("Palavra não encontrada.");
            }

            var favorite = new Favorite
            {
                UserId = userId,
                WordId = wordName
            };

            await _favoriteRepository.AddAsync(favorite);
        }

        public async Task RemoveWordFromFavoritesAsync(string wordName)
        {
            var userId = _jwtTokenService.GetUserIdFromToken();

            var favorite = await _favoriteRepository.GetByUserIdAndWordNameAsync(userId, wordName);

            if (favorite == null)
            {
                throw new Exception("Palavra não está em seus favoritos.");
            }

            await _favoriteRepository.RemoveAsync(favorite);
        }

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


    }


}
