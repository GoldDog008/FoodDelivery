using AutoMapper;
using Ipz.Models.Dto;
using Ipz.Models.Dto.Auth;
using Ipz.Services;
using Ipz.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ipz.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registerRequest)
        {
            try
            {
                var response = await _authService.RegisterAsync(registerRequest);
                Log.Information($"User with email {registerRequest.Email} was registered");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while register");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var response = await _authService.AuthenticateAsync(loginRequest);
                Log.Information($"User with email {loginRequest.Email} was login");

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
