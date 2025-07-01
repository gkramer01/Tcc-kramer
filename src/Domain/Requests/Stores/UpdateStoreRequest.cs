using Domain.Entities;
using Domain.Enums;

namespace Domain.Requests.Stores
{
    public class UpdateStoreRequest
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public List<Brand> Brands { get; set; } = [];
        public List<PaymentConditions> PaymentConditions { get; set; } = [];
    }
}
