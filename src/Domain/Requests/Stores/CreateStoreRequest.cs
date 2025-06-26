using Domain.Enums;
using Domain.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.Stores
{
    public class CreateStoreRequest
    {
        [Required(ErrorMessage = "Store name is required.")]
        [MaxLength(100, ErrorMessage = "Store name cannot exceed 100 characters.")]
        public required string Name { get; set; }

        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        [FlexibleUrl(ErrorMessage = "Website must be a valid URL.")]
        public string? Website { get; set; }

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        [Required(ErrorMessage = "Latitude is required.")]
        public required double Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        [Required(ErrorMessage = "Longitude is required.")]
        public required double Longitude { get; set; }

        [Required(ErrorMessage = "At least one brand is required.")]
        public List<string> Brands { get; set; } = [];

        [Required(ErrorMessage = "At least one payment condition is required.")]
        public required List<PaymentConditions> PaymentConditions { get; set; }
    }
}
