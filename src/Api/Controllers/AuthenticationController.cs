using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Requests;
using Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] AuthenticationRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Login and password are required." });
            }

            var user = await authenticationService.RegisterAsync(request);

            if (user is null)
            {
                return BadRequest("Username already exists.");
            }

            return Created("User has been created.", user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login([FromBody] AuthenticationRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { Message = "Username and password are required." });
            }

            var tokenResponse = await authenticationService.LoginAsync(request);

            if (!string.IsNullOrEmpty(tokenResponse!.Token))
            {
                return Ok(tokenResponse);
            }

            return BadRequest(new { Message = "Invalid username or password." });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var tokenResponse = await authenticationService.RefreshTokensAsync(request);
            if (tokenResponse is null || tokenResponse.Token is null || tokenResponse.RefreshToken is null)
            {
                return Unauthorized(new { Message = "Invalid refresh token." });
            }
            return Ok(tokenResponse);
        }

        [Authorize(Roles = nameof(RoleType.Admin))]
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return Ok("Autenticado");
        }
    }
}
