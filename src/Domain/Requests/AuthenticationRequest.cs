﻿namespace Domain.Requests
{
    public class AuthenticationRequest
    {
        public required string Username { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }
}
