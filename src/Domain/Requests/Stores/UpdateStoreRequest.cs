namespace Domain.Requests.Stores
{
    public class UpdateStoreRequest
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
    }
}
