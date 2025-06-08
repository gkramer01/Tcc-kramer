using Domain.Entities;
using Domain.Requests;
using Domain.Responses;
using Google.Apis.Auth;

namespace Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenResponse?> LoginAsync(AuthenticationRequest request);
        Task<User?> RegisterAsync(AuthenticationRequest request);
        Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequest request);
        Task<TokenResponse?> GoogleLoginAsync(GoogleJsonWebSignature.Payload payload);
    }
}