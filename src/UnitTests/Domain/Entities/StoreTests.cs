using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Domain.Entities
{
    public class StoreTests
    {
        [Fact]
        public void CanCreateStore_WithRequiredProperties()
        {
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Minha Loja",
                Latitude = 10.0,
                Longitude = 20.0,
                PaymentConditions = [],
                Brands = []
            };

            Assert.Equal("Minha Loja", store.Name);
            Assert.Equal(10.0, store.Latitude);
            Assert.Equal(20.0, store.Longitude);
            Assert.Empty(store.PaymentConditions);
            Assert.Empty(store.Brands);
        }

        [Fact]
        public void AddBrand_ShouldAddBrandToStore()
        {
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = []
            };
            var brand = new Brand { Id = Guid.NewGuid(), Name = "Marca", Stores = [] };

            store.AddBrand(brand);

            Assert.Contains(brand, store.Brands);
        }

        [Fact]
        public void RemoveBrand_ShouldRemoveBrandFromStore()
        {
            var brand = new Brand { Id = Guid.NewGuid(), Name = "Marca", Stores = [] };
            var store = new Store
            {
                Id = Guid.NewGuid(),
                Name = "Loja",
                Latitude = 0,
                Longitude = 0,
                PaymentConditions = [],
                Brands = [brand]
            };

            store.RemoveBrand(brand);

            Assert.DoesNotContain(brand, store.Brands);
        }
    }
}
