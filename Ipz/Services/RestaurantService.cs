using AutoMapper;
using Ipz_server.Models;
using Ipz_server.Models.Database;
using Ipz_server.Models.Dto;
using Ipz_server.Models.Dto.Locations;
using Ipz_server.Models.Dto.Restaurants;
using Ipz_server.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly FoodDeliveryContext _context;
        private readonly IMapper _mapper;
        private readonly ILocationService _locationService;
        public RestaurantService(FoodDeliveryContext context, IMapper mapper, ILocationService locationService)
        {
            _context = context;
            _mapper = mapper;
            _locationService = locationService;
        }
        public async Task<ApiResponse> GetAllRestaurants()
        {
            var apiResponse = new ApiResponse();

            try
            {
                var restaurants = await _context.Restaurants
                    .AsNoTracking()
                    .Include(l => l.Location)
                    .Include(d => d.Dishes)
                    .ToListAsync();

                if (restaurants == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("Restaurants not found");

                    return apiResponse;
                }

                List<RestaurantResponseDto> restaurantResponse = [];
                foreach (var restaurant in restaurants)
                {
                    restaurantResponse.Add(_mapper.Map<RestaurantResponseDto>(restaurant));
                }

                apiResponse.Success = true;
                apiResponse.Data = restaurantResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting user");
                throw;
            }
        }

        public async Task<ApiResponse> GetRestaurantByIdAsync(Guid id)
        {
            var apiResponse = new ApiResponse();

            try
            {
                var restaurant = await _context.Restaurants
                    .AsNoTracking()
                    .Include(l => l.Location)
                    .Include(d => d.Dishes)
                    .FirstOrDefaultAsync(u => u.RestaurantId == id);

                if (restaurant == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("Restaurant not found");

                    return apiResponse;
                }

                var restaurantResponse = _mapper.Map<RestaurantResponseDto>(restaurant);

                apiResponse.Success = true;
                apiResponse.Data = restaurantResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting user");
                throw;
            }
        }

        public async Task<ApiResponse> CreateRestaurant(RestaurantCreateRequestDto request)
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
                var locationRequest = _mapper.Map<LocationCreateRequestDto>(request);
                var locationCreateResult = await _locationService.CreateLocation(locationRequest);

                if (!locationCreateResult.Success)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("Location creation failed");

                    return apiResponse;
                }

                var restaurant = new Restaurant
                {
                    RestaurantId = Guid.NewGuid(),
                    Name = request.Name,
                    Location = locationCreateResult.Data as Location,
                };

                await _context.Restaurants.AddAsync(restaurant);
                await _context.SaveChangesAsync();

                apiResponse.Success = true;
                apiResponse.Data = _mapper.Map<RestaurantResponseDto>(restaurant);

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating restaurant");
                throw;
            }
        }
    }
}
