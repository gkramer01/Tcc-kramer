using Domain.Enums;

namespace Domain.Responses
{
    public class StoreResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<BrandResponse> Brands { get; set; } = [];
        public List<PaymentConditions> PaymentConditions { get; set; } = [];
    }
}
