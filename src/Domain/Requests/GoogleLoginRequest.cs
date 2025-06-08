using System.Text.Json.Serialization;

namespace Domain.Requests
{
    public sealed class GoogleLoginRequest
    {
        [JsonPropertyName("idToken")]
        public string? IdToken { get; set; }
    }
}
