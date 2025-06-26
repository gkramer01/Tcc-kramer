using Domain.Enums;
using Domain.Interfaces;
using Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository repository) : ControllerBase
    {
        [Authorize(Roles = $"{nameof(RoleType.Admin)},{nameof(RoleType.Customer)},{nameof(RoleType.Shopkeeper)},{nameof(RoleType.Seller)}")]
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> Update(Guid id, [FromBody] UpdateUserRequest request)
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
