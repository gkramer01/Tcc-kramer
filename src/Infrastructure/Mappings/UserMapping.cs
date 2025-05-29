using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class UserMapping : BaseMapping<User>
    {
        public override string TableName => "User";

        protected override void MapEntity(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                .HasColumnName("name")
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(u => u.UserName)
                .HasColumnName("username")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            builder.Property(u => u.Role)
                .HasConversion<int>()
                .HasColumnName("role")
                .IsRequired();

            builder.Property(u => u.RefreshToken)
                .HasColumnName("refresh_token")
                .IsRequired(false);

            builder.Property(u => u.RefreshTokenExpiryTime)
                .HasColumnName("refresh_token_epiry_time").
                IsRequired(false);

            builder.Ignore(b => b.CreatedBy);

            builder.HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}
