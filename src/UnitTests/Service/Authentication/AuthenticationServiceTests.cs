using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Service.Authentication;

namespace UnitTests.Service.Authentication
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();

        private readonly AuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            var tokenSectionMock = new Mock<IConfigurationSection>();
            tokenSectionMock.Setup(s => s.Value)
                .Returns("abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#");

            _configurationMock.Setup(c => c.GetSection("AppSettings:Token")).Returns(tokenSectionMock.Object);
            _configurationMock.Setup(c => c.GetSection("AppSettings:Issuer"))
                              .Returns(Mock.Of<IConfigurationSection>(s => s.Value == "TestIssuer"));
            _configurationMock.Setup(c => c.GetSection("AppSettings:Audience"))
                              .Returns(Mock.Of<IConfigurationSection>(s => s.Value == "TestAudience"));

            _authService = new AuthenticationService(_userRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenUserNotFound()
        {
            // Arrange
            var request = new AuthenticationRequest { Username = "nonexistent", Password = "pwd" };
            _userRepositoryMock.Setup(r => r.GetByUsernameAsync(It.IsAny<string>()))
                               .ReturnsAsync((User?)null);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Nome",
                UserName = "test",
                PasswordHash = "hash",
                Email = "email@teste.com",
                Role = RoleType.Customer
            };

            var request = new AuthenticationRequest { Username = "test", Password = "wrong_password" };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("test"))
                               .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task LoginAsync_ReturnsToken_WhenCredentialsAreCorrect()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Nome",
                UserName = "test",
                Email = "email@teste.com",
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "test123"),
                Role = RoleType.Customer
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "test123");

            var request = new AuthenticationRequest
            {
                Username = "test",
                Password = "test123"
            };

            _userRepositoryMock.Setup(r => r.GetByUsernameAsync("test"))
                               .ReturnsAsync(user);

            _userRepositoryMock.Setup(r => r.UpdateUserAsync(It.IsAny<User>()))
                               .Returns(Task.CompletedTask);

            // Act
            var result = await _authService.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(string.IsNullOrEmpty(result!.Token));
            Assert.False(string.IsNullOrEmpty(result.RefreshToken));
        }


    }

}
