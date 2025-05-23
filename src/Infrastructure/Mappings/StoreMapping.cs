using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class StoreMapping : BaseMapping<Store>
    {
        public override string TableName => "Store";

        protected override void MapEntity(EntityTypeBuilder<Store> builder)
        {
            builder.Property(s => s.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Address)
                .HasColumnName("address")
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(s => s.Email)
                .HasColumnName("email")
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(s => s.Website)
                .HasColumnName("website")
                .IsRequired(false)
                .HasMaxLength(100);

            builder.Property(s => s.Latitude)
                .HasColumnName("latitude")
                .IsRequired()
                .HasPrecision(10, 8);

            builder.Property(s => s.Longitude)
                .HasColumnName("longitude")
                .IsRequired()
                .HasPrecision(11, 8);

            builder.HasMany(s => s.Brands)
               .WithMany(b => b.Stores)
               .UsingEntity<Dictionary<string, object>>(
                    "StoreBrand", //tabela de junção
                    join => join
                        .HasOne<Brand>()
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Restrict),
                    join => join
                        .HasOne<Store>()
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade),
                    join =>
                    {
                        join.HasKey("StoreId", "BrandId");
                        join.ToTable("StoreBrand");
                    }
                );
        }
    }
}
