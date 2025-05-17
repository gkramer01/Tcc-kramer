using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Mappings
{
    public abstract class BaseMapping<T> : IBaseMapping where T : BaseEntity
    {
        public abstract string TableName { get; }

        public void MapEntity(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<T>();
            MapBaseEntity(builder);
            MapEntity(builder);
        }

        protected abstract void MapEntity(EntityTypeBuilder<T> builder);

        private void MapBaseEntity(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(TableName);
            builder.HasKey(u => u.Id);

            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnName("criado_em").IsRequired();
            builder.Property(x => x.CreatedBy).HasColumnName("criado_por").IsRequired(false);
            builder.Property(x => x.UpdatedAt).HasColumnName("atualizado_em").IsRequired();
            builder.Property(x => x.UpdatedBy).HasColumnName("atualizado_por").IsRequired(false);
        }
    }
}
