using Moq;
using System;
using System.Threading.Tasks;
using DictionaryApp.Application.Dtos;
using DictionaryApp.Application.Services;
using DictionaryApp.Domain.Entities;
using DictionaryApp.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace DictionaryApp.Tests.Application
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            // Inicializando mocks
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();

            // Inicializando o AuthService com os mocks
            _authService = new AuthService(
                _mockUserRepository.Object,
                new Microsoft.Extensions.Configuration.ConfigurationBuilder().Build(),
                _mockPasswordHasher.Object
            );
        }

        [Fact]
        public async Task SignInAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var signInDto = new SignInDto { Email = "test@example.com", Password = "password123" };
            var user = new User { Id = "123", Email = signInDto.Email, Password = "hashedPassword" };

            _mockUserRepository.Setup(r => r.GetByEmailAsync(signInDto.Email))
                .ReturnsAsync(user);
            _mockPasswordHasher.Setup(p => p.VerifyHashedPassword(user, user.Password, signInDto.Password))
                .Returns(PasswordVerificationResult.Success);

            // Act
            var result = await _authService.SignInAsync(signInDto);

            // Assert
            Assert.NotNull(result);
            _mockUserRepository.Verify(r => r.GetByEmailAsync(signInDto.Email), Times.Once);
            _mockPasswordHasher.Verify(p => p.VerifyHashedPassword(user, user.Password, signInDto.Password), Times.Once);
        }

        [Fact]
        public async Task SignInAsync_ShouldReturnNull_WhenEmailDoesNotExist()
        {
            // Arrange
            var signInDto = new SignInDto { Email = "nonexistent@example.com", Password = "password123" };

            _mockUserRepository.Setup(r => r.GetByEmailAsync(signInDto.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = await _authService.SignInAsync(signInDto);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(r => r.GetByEmailAsync(signInDto.Email), Times.Once);
        }

        [Fact]
        public async Task SignInAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var signInDto = new SignInDto { Email = "test@example.com", Password = "wrongPassword" };
            var user = new User { Id = "123", Email = signInDto.Email, Password = "hashedPassword" };

            _mockUserRepository.Setup(r => r.GetByEmailAsync(signInDto.Email))
                .ReturnsAsync(user);
            _mockPasswordHasher.Setup(p => p.VerifyHashedPassword(user, user.Password, signInDto.Password))
                .Returns(PasswordVerificationResult.Failed);

            // Act
            var result = await _authService.SignInAsync(signInDto);

            // Assert
            Assert.Null(result);
            _mockUserRepository.Verify(r => r.GetByEmailAsync(signInDto.Email), Times.Once);
            _mockPasswordHasher.Verify(p => p.VerifyHashedPassword(user, user.Password, signInDto.Password), Times.Once);
        }

        [Fact]
        public async Task SignUpAsync_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var signUpDto = new SignUpDto { Email = "test@example.com", Password = "password123", Name = "Test User" };
            var existingUser = new User { Email = signUpDto.Email };

            _mockUserRepository.Setup(r => r.GetByEmailAsync(signUpDto.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _authService.SignUpAsync(signUpDto));
            Assert.Equal("Usuário já existe com esse email.", exception.Message);
            _mockUserRepository.Verify(r => r.GetByEmailAsync(signUpDto.Email), Times.Once);
        }

        [Fact]
        public async Task SignUpAsync_ShouldCreateNewUser_WhenEmailIsUnique()
        {
            // Arrange
            var signUpDto = new SignUpDto { Email = "newuser@example.com", Password = "password123", Name = "New User" };
            var existingUser = new User { Email = signUpDto.Email };
            _mockUserRepository.Setup(r => r.GetByEmailAsync(signUpDto.Email))
                .ReturnsAsync((User)null);

            // Act
            await _authService.SignUpAsync(signUpDto);

            // Assert
            _mockUserRepository.Verify(r => r.GetByEmailAsync(signUpDto.Email), Times.Once);
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == signUpDto.Email)), Times.Once);
        }
    }
}
