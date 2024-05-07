using Ipz.Models.Dto;
using Ipz.Models.Dto.Auth;
using Ipz_server.Models;

namespace Ipz.Services.IServices
{
    public interface IAuthService
    {
        public Task<ApiResponse> RegisterAsync(RegistrationRequestDto registerRequest);
        public Task<ApiResponse> AuthenticateAsync(LoginRequestDto loginRequest);
    }
}
