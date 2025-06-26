using Data.Repositories;
using Domain.Entities;
using Infrastructure.Context;
using UnitTests.Config;

namespace UnitTests.Data.Repositories
{
    public class BrandsRepositoryTests
    {
        private readonly DefaultDbContext _context;
        private readonly BrandsRepository _repository;

        public BrandsRepositoryTests()
        {
            _context = TestsDbConfig.GetDbContext();
            _repository = new BrandsRepository(_context);
        }

        private void ClearDb()
        {
            _context.Brands.RemoveRange(_context.Brands);
            _context.Stores.RemoveRange(_context.Stores);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllBrandsAsync_ShouldReturnAllBrands()
        {
            // Arrange
            ClearDb();
            _context.Brands.Add(new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Marca1",
                Stores = new List<Store>()
            });
            _context.Brands.Add(new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Marca2",
                Stores = new List<Store>()
            });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAllBrandsAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdsListAsync_ShouldReturnFilteredBrands()
        {
            // Arrange
            ClearDb();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var idsList = new List<Guid> { id1, id2 };

            _context.Brands.Add(new Brand
            {
                Id = id1,
                Name = "Marca1",
                Stores = []
            });
            _context.Brands.Add(new Brand
            {
                Id = id2,
                Name = "Marca2",
                Stores = []
            });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetByIdsListAsync(idsList);

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}