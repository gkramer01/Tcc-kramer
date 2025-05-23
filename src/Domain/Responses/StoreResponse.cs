using Domain.Entities;

namespace Domain.Responses
{
    public class StoreResponse
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<BrandResponse> Brands { get; set; } = [];
    }
}
