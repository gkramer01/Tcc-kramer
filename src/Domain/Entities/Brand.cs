using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Brand : BaseEntity
    {
        public required string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Store> Stores { get; set; } = [];
    }
}
