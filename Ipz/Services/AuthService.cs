using AutoMapper;
using Ipz.Models.Dto.Auth;
using Ipz.Services.IServices;
using Ipz_server.Models;
using Ipz_server.Models.Database;
using Ipz_server.Models.Dto.Auth;
using Ipz_server.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Ipz.Services
{
    public class AuthService : IAuthService
    {
        private readonly FoodDeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(FoodDeliveryContext context, 
            IMapper mapper, 
            ITokenService tokenService,
            IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<ApiResponse> AuthenticateAsync(LoginRequestDto loginRequest)
        {
            var apiResponse = new ApiResponse();
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(loginRequest, new ValidationContext(loginRequest), validationResults, true))
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Invalid model state");

                return apiResponse;
            }

            try
            {
                var user = await _context.Users
                    .Include(l => l.Location)
                    .Include(r => r.Role)
                    .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

                if (user == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("Incorrect email or password");

                    return apiResponse;
                }

                var userResponse = _mapper.Map<UserAuthResponseDto>(user);
                userResponse.AccessToken = _tokenService.CreateAccessToken(user);

                apiResponse.Success = true;
                apiResponse.Data = userResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while authenticate");
                throw;
            }
        }

        public async Task<ApiResponse> RegisterAsync(RegistrationRequestDto registerRequest)
        {
            var apiResponse = new ApiResponse();
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(registerRequest, new ValidationContext(registerRequest), validationResults, true))
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Invalid model state");

                return apiResponse;
            }

            try
            {
                var isExistsUser = await _context.Users.AnyAsync(u => u.Email == registerRequest.Email);

                if (isExistsUser)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("User already exists");

                    return apiResponse;
                }

                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

                if (userRole == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("User role does not found");

                    return apiResponse;
                }

                User user = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    Email = registerRequest.Email,
                    Password = registerRequest.Password,
                    Role = userRole,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                await _userService.SaveUserToFile(user);

                var userResponse = _mapper.Map<UserAuthResponseDto>(user);
                userResponse.AccessToken = _tokenService.CreateAccessToken(user);

                apiResponse.Success = true;
                apiResponse.Data = userResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while registering");
                throw;
            }
        }
    }
}
