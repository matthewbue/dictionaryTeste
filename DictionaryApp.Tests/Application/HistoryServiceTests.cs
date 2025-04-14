using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using DictionaryApp.Application.Services;
using Xunit;

namespace DictionaryApp.Tests.Application
{
    public class HistoryServiceTests
    {
        private readonly Mock<IHistoryRepository> _mockHistoryRepository;
        private readonly HistoryService _historyService;

        public HistoryServiceTests()
        {
            _mockHistoryRepository = new Mock<IHistoryRepository>();

            _historyService = new HistoryService(_mockHistoryRepository.Object);
        }

        [Fact]
        public async Task AddToHistoryAsync_ShouldAddHistory_WhenCalled()
        {
            // Arrange
            var userId = "user123";
            var word = "example";

            _mockHistoryRepository.Setup(r => r.AddHistoryAsync(userId, word))
                .Returns(Task.CompletedTask);

            // Act
            await _historyService.AddToHistoryAsync(userId, word);

            // Assert
            _mockHistoryRepository.Verify(r => r.AddHistoryAsync(userId, word), Times.Once);
        }

        [Fact]
        public async Task ClearHistoryAsync_ShouldClearHistory_WhenCalled()
        {
            // Arrange
            var userId = "user123";

            _mockHistoryRepository.Setup(r => r.ClearHistoryAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _historyService.ClearHistoryAsync(userId);

            // Assert
            _mockHistoryRepository.Verify(r => r.ClearHistoryAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetHistoryAsync_ShouldReturnHistory_WhenHistoryExists()
        {
            // Arrange
            var userId = "user123";
            var history = new List<History>
            {
                new History { Word = "example", Added = DateTime.UtcNow.AddDays(-1) },
                new History { Word = "sample", Added = DateTime.UtcNow.AddDays(-2) }
            };

            _mockHistoryRepository.Setup(r => r.GetHistoryAsync(userId))
                .ReturnsAsync(history);

            // Act
            var result = await _historyService.GetHistoryAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, h => h.Word == "example");
            Assert.Contains(result, h => h.Word == "sample");
            _mockHistoryRepository.Verify(r => r.GetHistoryAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetHistoryAsync_ShouldReturnEmpty_WhenNoHistoryExists()
        {
            // Arrange
            var userId = "user123";
            var history = new List<History>();

            _mockHistoryRepository.Setup(r => r.GetHistoryAsync(userId))
                .ReturnsAsync(history);

            // Act
            var result = await _historyService.GetHistoryAsync(userId);

            // Assert
            Assert.Empty(result);
            _mockHistoryRepository.Verify(r => r.GetHistoryAsync(userId), Times.Once);
        }
    }
}
