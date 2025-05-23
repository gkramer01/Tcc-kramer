using FluentValidation;

namespace Domain.Entities
{
    public class Store : BaseEntity
    {
        public required string Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public ICollection<Brand> Brands { get; set; } = [];

        public void AddBrand(Brand brand)
        {
            Brands ??= [];
            Brands.Add(brand);
        }

        public void RemoveBrand(Brand brand)
        {
            if (Brands == null) return;
            Brands.Remove(brand);
        }
    }

    public class StoreValidator : AbstractValidator<Store>
    {
        public StoreValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(s => s.Email)
                .EmailAddress().When(s => !string.IsNullOrEmpty(s.Email))
                .WithMessage("Email is not valid.");

            RuleFor(s => s.Website)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .When(s => !string.IsNullOrEmpty(s.Website))
                .WithMessage("Website URL is not valid.");

            RuleFor(s => s.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

            RuleFor(s => s.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }
}
