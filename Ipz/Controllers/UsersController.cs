using Ipz;
using Ipz_server.Models.Dto.Users;
using Ipz_server.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ipz_server.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var response = await _userService.GetUserByIdAsync(id);
                Log.Information($"User with id {id} was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequestDto request)
        {
            var currentUserId = Guid.Parse(HttpContext.GetCurrentUserId());

            try
            {
                var response = await _userService.UpdateUserAsync(currentUserId, request);
                Log.Information($"User update with id {currentUserId} was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while register");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
