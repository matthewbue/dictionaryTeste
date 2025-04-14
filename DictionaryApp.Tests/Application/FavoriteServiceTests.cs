using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Services;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using Xunit;

namespace DictionaryApp.Tests.Application
{
    public class FavoriteServiceTests
    {
        private readonly Mock<IFavoriteRepository> _mockFavoriteRepository;
        private readonly FavoriteService _favoriteService;

        public FavoriteServiceTests()
        {
            _mockFavoriteRepository = new Mock<IFavoriteRepository>();

            _favoriteService = new FavoriteService(_mockFavoriteRepository.Object);
        }

        [Fact]
        public async Task GetFavoriteWordsAsync_ShouldReturnFavoriteWords_WhenUserHasFavorites()
        {
            // Arrange
            var userId = "user123";
            var favorites = new List<Favorite>
            {
                new Favorite { WordId = "word1", Added = DateTime.UtcNow.AddDays(-1) },
                new Favorite { WordId = "word2", Added = DateTime.UtcNow.AddDays(-2) }
            };

            _mockFavoriteRepository.Setup(r => r.GetFavoritesAsync(userId))
                .ReturnsAsync(favorites);

            // Act
            var result = await _favoriteService.GetFavoriteWordsAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, f => f.WordId == "word1");
            Assert.Contains(result, f => f.WordId == "word2");
            _mockFavoriteRepository.Verify(r => r.GetFavoritesAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetFavoriteWordsAsync_ShouldReturnEmpty_WhenUserHasNoFavorites()
        {
            // Arrange
            var userId = "user123";
            var favorites = new List<Favorite>();

            _mockFavoriteRepository.Setup(r => r.GetFavoritesAsync(userId))
                .ReturnsAsync(favorites);

            // Act
            var result = await _favoriteService.GetFavoriteWordsAsync(userId);

            // Assert
            Assert.Empty(result);
            _mockFavoriteRepository.Verify(r => r.GetFavoritesAsync(userId), Times.Once);
        }
    }
}
