using Domain.Requests.Stores;

namespace UnitTests.Domain.Requests.Stores
{
    public class UpdateStoreRequestTests
    {
        [Fact]
        public void CanCreateUpdateStoreRequest_WithAllProperties()
        {
            var request = new UpdateStoreRequest
            {
                Name = "Loja Atualizada",
                Email = "nova@email.com",
                Website = "https://novaloja.com"
            };

            Assert.Equal("Loja Atualizada", request.Name);
            Assert.Equal("nova@email.com", request.Email);
            Assert.Equal("https://novaloja.com", request.Website);
        }
    }
}
