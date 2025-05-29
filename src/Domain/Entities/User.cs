using Domain.Enums;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Name { get; set; } = string.Empty;
        public required string UserName { get; set; } = string.Empty;
        public required string PasswordHash { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required RoleType Role { get; set; } = RoleType.Customer;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
