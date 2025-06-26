using Domain.Entities;

namespace UnitTests.Domain.Entities
{
    public class BrandTests
    {
        [Fact]
        public void CanCreateBrand_WithRequiredProperties()
        {
            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = "Marca",
                Stores = []
            };

            Assert.Equal("Marca", brand.Name);
            Assert.Empty(brand.Stores);
        }
    }
}
