using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Services;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using Xunit;

namespace DictionaryApp.Tests.Application
{
    public class WordServiceTests
    {
        private readonly Mock<IWordRepository> _mockWordRepository;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly Mock<IHistoryRepository> _mockHistoryRepository;
        private readonly Mock<IFavoriteRepository> _mockFavoriteRepository;
        private readonly Mock<IJwtTokenService> _mockJwtTokenService;
        private readonly WordService _wordService;

        public WordServiceTests()
        {
            _mockWordRepository = new Mock<IWordRepository>();
            _mockCacheService = new Mock<ICacheService>();
            _mockHistoryRepository = new Mock<IHistoryRepository>();
            _mockFavoriteRepository = new Mock<IFavoriteRepository>();
            _mockJwtTokenService = new Mock<IJwtTokenService>();

            _mockJwtTokenService.Setup(j => j.GetUserIdFromToken()).Returns("userId");

            _wordService = new WordService(
                _mockWordRepository.Object,
                _mockHistoryRepository.Object,
                _mockFavoriteRepository.Object,
                _mockJwtTokenService.Object,
                _mockCacheService.Object
            );
        }

        [Fact]
        public async Task GetWordDetailsAsync_ShouldReturnFromCache_WhenWordExistsInCache()
        {
            // Arrange
            var word = "example";
            var wordDto = new WordDto { WordName = word, Definition = "A sample word" };
            var cacheKey = $"word:{word.ToLower()}";

            _mockCacheService.Setup(c => c.GetAsync<WordDto>(cacheKey))
                .ReturnsAsync(wordDto);

            // Act
            var result = await _wordService.GetWordDetailsAsync(word);

            // Assert
            Assert.Equal(word, result.WordName);
            Assert.Equal("A sample word", result.Definition);
            _mockCacheService.Verify(c => c.GetAsync<WordDto>(cacheKey), Times.Once);
            _mockWordRepository.Verify(r => r.GetWordByNameAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetWordDetailsAsync_ShouldReturnFromRepository_WhenNotInCache()
        {
            // Arrange
            var word = "example";
            var wordDto = new WordDto { WordName = word, Definition = "A sample word" };
            var wordEntity = new Word { WordName = word, Definition = "A sample word" };
            var cacheKey = $"word:{word.ToLower()}";

            _mockCacheService.Setup(c => c.GetAsync<WordDto>(cacheKey))
                .ReturnsAsync((WordDto)null);
            _mockWordRepository.Setup(r => r.GetWordByNameAsync(word))
                .ReturnsAsync(wordEntity);

            // Act
            var result = await _wordService.GetWordDetailsAsync(word);

            // Assert
            Assert.Equal(word, result.WordName);
            Assert.Equal("A sample word", result.Definition);
            _mockCacheService.Verify(c => c.GetAsync<WordDto>(cacheKey), Times.Once);
            _mockWordRepository.Verify(r => r.GetWordByNameAsync(word), Times.Once);
            _mockCacheService.Verify(c => c.SetAsync(cacheKey, It.IsAny<WordDto>(), It.IsAny<TimeSpan?>()), Times.Once);
        }

        [Fact]
        public async Task SearchWordsAsync_ShouldReturnWordsBasedOnSearch()
        {
            // Arrange
            var search = "example";
            var limit = 10;
            var page = 1;
            var words = new List<Word>
            {
                new Word { WordName = "example", Definition = "A sample word" },
                new Word { WordName = "example2", Definition = "Another word" }
            };

            _mockWordRepository.Setup(r => r.GetWordsAsync(search, limit, page))
                .ReturnsAsync(words);

            // Act
            var result = await _wordService.SearchWordsAsync(search, limit, page);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, w => w.WordName == "example");
        }

        [Fact]
        public async Task GetFavoriteWordsAsync_ShouldReturnFavorites()
        {
            // Arrange
            var favorites = new List<Favorite>
            {
                new Favorite { WordId = "example", Added = DateTime.UtcNow }
            };

            _mockFavoriteRepository.Setup(f => f.GetFavoritesAsync(It.IsAny<string>()))
                .ReturnsAsync(favorites);

            // Act
            var result = await _wordService.GetFavoriteWordsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("example", result.First().WordId);
        }

        [Fact]
        public async Task AddWordToFavoritesAsync_ShouldAddWordToFavorites()
        {
            // Arrange
            var wordName = "example";
            var userId = "userId";
            var word = new Word { WordName = wordName, Definition = "A sample word" };

            _mockJwtTokenService.Setup(j => j.GetUserIdFromToken()).Returns(userId);
            _mockWordRepository.Setup(w => w.GetWordByNameAsync(wordName)).ReturnsAsync(word);

            // Act
            await _wordService.AddWordToFavoritesAsync(wordName);

            // Assert
            _mockFavoriteRepository.Verify(f => f.AddAsync(It.Is<Favorite>(fav => fav.UserId == userId && fav.WordId == wordName)), Times.Once);
        }

        [Fact]
        public async Task RemoveWordFromFavoritesAsync_ShouldRemoveWordFromFavorites()
        {
            // Arrange
            var wordName = "example";
            var userId = "userId";
            var favorite = new Favorite { WordId = wordName, UserId = userId };

            _mockJwtTokenService.Setup(j => j.GetUserIdFromToken()).Returns(userId);
            _mockFavoriteRepository.Setup(f => f.GetByUserIdAndWordNameAsync(userId, wordName)).ReturnsAsync(favorite);

            // Act
            await _wordService.RemoveWordFromFavoritesAsync(wordName);

            // Assert
            _mockFavoriteRepository.Verify(f => f.RemoveAsync(favorite), Times.Once);
        }
    }
}
