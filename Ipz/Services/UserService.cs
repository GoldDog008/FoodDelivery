using AutoMapper;
using Ipz_server.Models;
using Ipz_server.Models.Database;
using Ipz_server.Models.Dto;
using Ipz_server.Models.Dto.Auth;
using Ipz_server.Models.Dto.Locations;
using Ipz_server.Models.Dto.Users;
using Ipz_server.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Ipz_server.Services
{
    public class UserService : IUserService
    {
        private readonly FoodDeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly ILocationService _locationService;

        public UserService(FoodDeliveryContext context,
            IMapper mapper,
            ILocationService locationService)
        {
            _context = context;
            _mapper = mapper;
            _locationService = locationService;
        }

        public async Task<ApiResponse> GetUserByIdAsync(Guid id)
        {
            var apiResponse = new ApiResponse();

            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .Include(l => l.Location)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("User not found");

                    return apiResponse;
                }

                var userResponse = _mapper.Map<UserResponseDto>(user);

                apiResponse.Success = true;
                apiResponse.Data = userResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting user");
                throw;
            }
        }

        public async Task<ApiResponse> UpdateUserAsync(Guid id, UserUpdateRequestDto request)
        {
            var apiResponse = new ApiResponse();
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
            {
                apiResponse.Success = false;
                apiResponse.Errors.Add("Invalid model state");

                return apiResponse;
            }

            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .Include(l => l.Location)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("User not found");

                    return apiResponse;
                }

                UpdateUserInfo(request, user);
                await CreateOrUpdateLocation(request, user);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                await SaveUserToFile(user);

                apiResponse.Success = true;
                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while updating user");
                throw;
            }
        }

        public async Task<ApiResponse> IsAllDataFilledInAsync(Guid id)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .Include(l => l.Location)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (user == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Errors = new List<string> { "User not found" }
                    };
                }

                if (user.FirstName == null ||
                    user.LastName == null ||
                    user.Email == null ||
                    user.Phone == null ||
                    user.Location == null ||
                    user.Location.City == null ||
                    user.Location.Country == null ||
                    user.Location.Street == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Errors = new List<string> { "Not all data is filled in" }
                    };
                }

                return new ApiResponse
                {
                    Success = true
                };

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while checking if all data is filled in");
                throw;
            }
        }

        public async Task<ApiResponse> SaveUserToFile(User user)
        {
            try
            {
                string filePath = Path.Combine("data", $"{user.UserId}.json");
                string jsonData = JsonSerializer.Serialize(new 
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Phone,
                });

                string directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                await File.WriteAllTextAsync(filePath, jsonData);

                return new ApiResponse
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while saving user to file");
                throw;
            }
        }


        private static void UpdateUserInfo(UserUpdateRequestDto request, User user)
        {
            if (!request.FirstName.IsNullOrEmpty())
            {
                user.FirstName = request.FirstName;
            }
            if (!request.LastName.IsNullOrEmpty())
            {
                user.LastName = request.LastName;
            }
            if (!request.Phone.IsNullOrEmpty())
            {
                user.Phone = request.Phone;
            }
        }

        private async Task CreateOrUpdateLocation(UserUpdateRequestDto request, User user)
        {
            if (user.Location == null)
            {
                var locationRequest = new LocationCreateRequestDto
                {
                    Country = request.Country,
                    City = request.City,
                    Street = request.Street
                };

                var locationResponse = await _locationService.CreateLocation(locationRequest);

                if (locationResponse.Success)
                {
                    user.Location = locationResponse.Data as Location;
                }
            }
            else
            {
                var locationRequest = new LocationUpdateRequestDto
                {
                    LocationId = user.Location.LocationId,
                    Country = request.Country,
                    City = request.City,
                    Street = request.Street
                };

                var locationResponse = await _locationService.UpdateLocation(locationRequest);

                if (locationResponse.Success)
                {
                    user.Location = locationResponse.Data as Location;
                }
            }
        }
    }
}
