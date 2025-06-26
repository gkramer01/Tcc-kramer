using Data.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using UnitTests.Config;

namespace UnitTests.Data.Repositories
{
    public class UserRepositoryTests
    {
        private readonly DefaultDbContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _context = TestsDbConfig.GetDbContext();
            _repository = new UserRepository(_context);
        }

        private void ClearDb()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddUser_ShouldAddUserToDatabase()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = id,
                Name = "Teste",
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "",
                Role = RoleType.Customer
            };

            // Act
            await _repository.AddUserAsync(user);

            // Assert
            var createdUser = await _repository.GetByIdAsync(id);

            Assert.NotNull(createdUser);
            Assert.Equal(id, createdUser.Id);
            Assert.Equal("Teste", createdUser.Name);
            Assert.Equal("testuser", createdUser.UserName);
            Assert.Equal("hashedpassword", createdUser.PasswordHash);
            Assert.Equal(RoleType.Customer, createdUser.Role);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Teste",
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "",
                Role = RoleType.Customer
            };

            await _repository.AddUserAsync(user);

            // Act
            var result = await _repository.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByUsernameAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Teste",
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "",
                Role = RoleType.Customer
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetByUsernameAsync("testuser");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUsers()
        {
            // Arrange
            ClearDb();

            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "User1",
                UserName = "user1",
                PasswordHash = "hash1",
                Email = "",
                Role = RoleType.Customer
            });
            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "User2",
                UserName = "user2",
                PasswordHash = "hash2",
                Email = "",
                Role = RoleType.Customer
            });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WhenUserExists()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Teste",
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "",
                Role = RoleType.Customer
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var exists = await _repository.ExistsAsync("testuser");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Act
            var exists = await _repository.ExistsAsync("notfound");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Teste",
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "",
                Role = RoleType.Customer
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            user.Name = "Atualizado";
            await _repository.UpdateUserAsync(user);

            // Assert
            var updatedUser = _context.Users.First(u => u.Id == user.Id);
            Assert.Equal("Atualizado", updatedUser.Name);
            Assert.True(updatedUser.UpdatedAt > user.CreatedAt);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Teste",
                UserName = "testuser",
                PasswordHash = "hashedpassword",
                Email = "",
                Role = RoleType.Customer
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            await _repository.DeleteUserAsync(user.Id);

            // Assert
            Assert.Empty(_context.Users);
        }
    }
}