using DictionaryApp.Application.Services;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using Moq;

namespace DictionaryApp.Tests.Application
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();

            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetCurrentUserAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = "user123";
            var user = new User { Id = userId, Name = "John Doe", Email = "john.doe@example.com" };

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetCurrentUserAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john.doe@example.com", result.Email);
            _mockUserRepository.Verify(r => r.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetCurrentUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = "nonexistentUserId";

            _mockUserRepository.Setup(r => r.GetByIdAsync(userId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetCurrentUserAsync(userId);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(r => r.GetByIdAsync(userId), Times.Once);
        }
    }
}
