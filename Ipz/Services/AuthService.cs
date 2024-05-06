using AutoMapper;
using Ipz.Models.Database;
using Ipz.Models.Dto;
using Ipz.Models.Dto.Auth;
using Ipz.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Ipz.Services
{
    public class AuthService : IAuthService
    {
        private readonly FoodDeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(FoodDeliveryContext context, IMapper mapper, ITokenService tokenService)
        {
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<AuthUserResponseDto?> AuthenticateAsync(LoginRequestDto loginRequest)
        {
            try
            {
                var user = await _context.Users
                    .Include(l => l.Location)
                    .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

                if (user == null)
                {
                    return null;
                }

                var userResponse = _mapper.Map<AuthUserResponseDto>(user);
                userResponse.AccessToken = _tokenService.CreateAccessToken(user);

                return userResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while authenticate");
                throw;
            }
        }

        public async Task<AuthUserResponseDto?> RegisterAsync(RegistrationRequestDto registerRequest)
        {
            try
            {
                var isExistsUser = await _context.Users.AnyAsync(u => u.Email == registerRequest.Email);

                if (isExistsUser)
                {
                    return null;
                }

                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

                if (userRole == null)
                {
                    throw new InvalidOperationException("User role does not found");
                }

                User user = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    Password = registerRequest.Password,
                    RoleId = userRole.RoleId,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var userResponse = _mapper.Map<AuthUserResponseDto>(user);
                userResponse.AccessToken = _tokenService.CreateAccessToken(user);

                return userResponse;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "User role does not found");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering");
                throw;
            }
        }
    }
}
