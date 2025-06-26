namespace Domain.Requests
{
    public class UpdateUserRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
    }
}
