using Data.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using UnitTests.Config;

namespace UnitTests.Data.Repositories
{
    public class StoreRepositoryTests
    {
        private readonly DefaultDbContext _context;
        private readonly StoreRepository _repository;

        public StoreRepositoryTests()
        {
            _context = TestsDbConfig.GetDbContext();
            _repository = new StoreRepository(_context);
        }

        private void ClearDb()
        {
            _context.Stores.RemoveRange(_context.Stores);
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddStore_ShouldAddStoreToDatabase()
        {
            // Arrange
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja Teste",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = [],
                IsDeleted = false
            };

            // Act
            await _repository.AddStoreAsync(store);

            // Assert
            var createdStore = await _repository.GetByIdAsync(store.Id);
            Assert.NotNull(createdStore);
            Assert.Equal(store.Id, createdStore.Id);
            Assert.Equal("Loja Teste", createdStore.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnStore_WhenStoreExists()
        {
            // Arrange
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja Teste",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = []
            };
            await _repository.AddStoreAsync(store);

            // Act
            var result = await _repository.GetByIdAsync(store.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(store.Id, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllStores()
        {
            // Arrange
            ClearDb();
            _context.Stores.Add(new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja1",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = []
            });
            _context.Stores.Add(new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja2",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = []
            });
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task UpdateStoreAsync_ShouldUpdateStore()
        {
            // Arrange
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja Teste",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = []
            };
            _context.Stores.Add(store);
            _context.SaveChanges();

            // Act
            store.Name = "Atualizada";
            await _repository.UpdateStoreAsync(store);

            // Assert
            var updatedStore = _context.Stores.First(s => s.Id == store.Id);
            Assert.Equal("Atualizada", updatedStore.Name);
        }

        [Fact]
        public async Task DeleteStoreAsync_ShouldRemoveStore()
        {
            // Arrange
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja Teste",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = []
            };
            _context.Stores.Add(store);
            _context.SaveChanges();

            // Act
            await _repository.DeleteStoreAsync(store.Id);

            // Assert
            var stores = await _repository.GetAllAsync();
            Assert.Empty(stores);
        }
    }
}