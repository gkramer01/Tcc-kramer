using Domain.Entities;
using Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var mappingTypes = typeof(IBaseMapping).Assembly
                .GetTypes()
                .Where(t => typeof(IBaseMapping).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in mappingTypes)
            {
                if (Activator.CreateInstance(type) is IBaseMapping mappingInstance)
                {
                    mappingInstance.MapEntity(modelBuilder);
                }
            }
        }
    }
}
