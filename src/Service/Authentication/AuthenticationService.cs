using Domain.Entities;
using Domain.Interfaces;
using Domain.Requests;
using Domain.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service.Authentication
{
    public class AuthenticationService(IUserRepository userRepository, IConfiguration configuration) : IAuthenticationService
    {
        public async Task<TokenResponse?> LoginAsync(AuthenticationRequest request)
        {
            var user = await userRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                return null;
            }

            var passwordVerified = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordVerified == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return await CreateTokenResponseAsync(user);
        }

        public async Task<User?> RegisterAsync(AuthenticationRequest request)
        {
            var exists = await userRepository.ExistsAsync(request.Username);

            if (exists)
                return null;

            var user = new User
            {
                Name = string.Empty,
                UserName = request.Username,
                PasswordHash = string.Empty,
                Email = string.Empty,
                Role = Domain.Enums.RoleType.Customer
            };

            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);

            await userRepository.AddUserAsync(user);

            return user;
        }

        public async Task<TokenResponse?> RefreshTokensAsync(RefreshTokenRequest request)
        {
            var user = ValidateRefreshToken(request.UserId, request.RefreshToken!).Result;

            if (user == null)
            {
                return null;
            }

            return await CreateTokenResponseAsync(user);
        }

        private async Task<TokenResponse> CreateTokenResponseAsync(User user)
        {
            return new TokenResponse
            {
                Token = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }
        #region Refresh Token
        private async Task<User?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>{
                new (ClaimTypes.Name, user.UserName),
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role.ToString())
            };

            if (user.Role == Domain.Enums.RoleType.Admin)
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds,
                claims: claims
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userRepository.UpdateUserAsync(user);
            return refreshToken;
        }
        #endregion
    }
}
