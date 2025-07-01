using System.Text.Json.Serialization;

namespace Domain.Requests
{
    public class UpdateUserRequest
    {
        [JsonPropertyName("Name")]
        public required string Name { get; set; }

        [JsonPropertyName("Email")]
        public required string Email { get; set; }

        [JsonPropertyName("UserName")]
        public required string UserName { get; set; }
    }
}
