using Ipz.Models.Dto;
using Ipz.Models.Dto.Auth;

namespace Ipz.Services.IServices
{
    public interface IAuthService
    {
        public Task<AuthUserResponseDto?> RegisterAsync(RegistrationRequestDto registerRequest);
        public Task<AuthUserResponseDto?> AuthenticateAsync(LoginRequestDto loginRequest);
    }
}
