using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public class BrandMapping : BaseMapping<Brand>
    {
        public override string TableName => "Brand";
        protected override void MapEntity(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(b => b.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);

            builder.Ignore(b => b.CreatedAt);
            builder.Ignore(b => b.UpdatedAt);
            builder.Ignore(b => b.DeletedAt);
            builder.Ignore(b => b.IsDeleted);
        }
    }
}
