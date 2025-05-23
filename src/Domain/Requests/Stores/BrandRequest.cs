using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.Stores
{
    public class BrandRequest
    {
        [Required(ErrorMessage = "Brand ID is required.")]
        public required Guid Id { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [MaxLength(50, ErrorMessage = "Brand name cannot exceed 100 characters.")]
        public required string Name { get; set; }
    }
}
