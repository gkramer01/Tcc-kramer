using Domain.Entities;
using Domain.Requests;
using Domain.Responses;

namespace Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenResponse?> LoginAsync(AuthenticationRequest request);
        Task<User?> RegisterAsync(AuthenticationRequest request);
        Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequest request);
    }
}