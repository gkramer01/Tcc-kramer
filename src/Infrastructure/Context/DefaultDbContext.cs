using Domain.Entities;
using Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var mappingTypes = typeof(UserMapping).Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IBaseMapping)))
            .Where(x => !x.IsAbstract)
            .ToList();

            foreach (var mappingType in mappingTypes)
            {
                var mapping = Activator.CreateInstance(mappingType);

                var initializeMethod = mapping!.GetType().GetMethod(nameof(IBaseMapping.MapEntity));

                initializeMethod!.Invoke(mapping, [modelBuilder]);
            }
        }
    }
}
