using Domain.Interfaces;
using Domain.Requests.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository repository) : ControllerBase
    {
        [HttpGet("user/{id}")]
        public async Task<ActionResult<string>> Update([FromBody] UpdateUserRequest request, Guid id)
        {
            var user = await repository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if(!ValidateRequest(request))
            {
                return BadRequest(new { Message = "Invalid request data.", Request = request });
            }

            user.Name = request.Name;
            user.Email = request.Email;

            if (!string.IsNullOrEmpty(request.UserName))
            {
                user.UserName = request.UserName;
            }

            await repository.UpdateUserAsync(user);

            return Accepted(id);
        }

        private static bool ValidateRequest(UpdateUserRequest request)
        {
            return request != null &&
                   !string.IsNullOrWhiteSpace(request.Name) &&
                   !string.IsNullOrWhiteSpace(request.Email);
        }
    }
}
