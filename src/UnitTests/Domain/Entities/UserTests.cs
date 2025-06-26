using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void CanCreateUser_WithRequiredProperties()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Nome",
                UserName = "usuario",
                PasswordHash = "hash",
                Email = "email@teste.com",
                Role = RoleType.Customer
            };

            Assert.Equal("Nome", user.Name);
            Assert.Equal("usuario", user.UserName);
            Assert.Equal("hash", user.PasswordHash);
            Assert.Equal("email@teste.com", user.Email);
            Assert.Equal(RoleType.Customer, user.Role);
        }
    }
}
