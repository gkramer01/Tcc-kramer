using System.ComponentModel.DataAnnotations;

namespace Domain.Requests.Stores
{
    public class BrandRequest
    {
        [Required(ErrorMessage = "Brand ID is required.")]
        public required Guid Id { get; set; }
    }
}
