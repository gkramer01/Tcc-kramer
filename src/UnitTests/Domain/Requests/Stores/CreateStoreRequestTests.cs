using Domain.Enums;
using Domain.Requests.Stores;

namespace UnitTests.Domain.Requests.Stores
{
    public class CreateStoreRequestTests
    {
        [Fact]
        public void CanCreateStoreRequest_WithAllProperties()
        {
            var request = new CreateStoreRequest
            {
                Name = "Minha Loja",
                Address = "Rua Teste, 123",
                Email = "loja@email.com",
                Website = "https://minhaloja.com",
                Latitude = -23.5,
                Longitude = -46.6,
                Brands = [ "1", "2" ],
                PaymentConditions = [ PaymentConditions.Cash, PaymentConditions.Pix ]
            };

            Assert.Equal("Minha Loja", request.Name);
            Assert.Equal("Rua Teste, 123", request.Address);
            Assert.Equal("loja@email.com", request.Email);
            Assert.Equal("https://minhaloja.com", request.Website);
            Assert.Equal(-23.5, request.Latitude);
            Assert.Equal(-46.6, request.Longitude);
            Assert.Contains("1", request.Brands);
            Assert.Contains("2", request.Brands);
            Assert.Contains(PaymentConditions.Cash, request.PaymentConditions);
        }
    }
}
