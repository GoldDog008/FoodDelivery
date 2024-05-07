using AutoMapper;
using Ipz_server.Models;
using Ipz_server.Models.Database;
using Ipz_server.Models.Dto;
using Ipz_server.Models.Dto.Dishes;
using Ipz_server.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Ipz_server.Services
{
    public class DishService : IDishService
    {
        private readonly FoodDeliveryContext _context;
        private readonly IMapper _mapper;

        public DishService(FoodDeliveryContext context,
            IMapper mapper,
            ILocationService locationService)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse> GetDishByIdAsync(Guid id)
        {
            var apiResponse = new ApiResponse();

            try
            {
                var dish = await _context.Dishes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.DishId == id);

                if (dish == null)
                {
                    apiResponse.Success = false;
                    apiResponse.Errors.Add("Dish not found");

                    return apiResponse;
                }

                var dishResponse = _mapper.Map<DishResponseDto>(dish);

                apiResponse.Success = true;
                apiResponse.Data = dishResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting dish");
                throw;
            }
        }

        public async Task<ApiResponse> AddDishToRestaurant(DishToRestaurantRequestDto request)
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
                var dish = await _context.Dishes
                    .Include(d => d.Restaurants)
                    .FirstOrDefaultAsync(d => d.Name == request.Name);

                if (dish != null)
                {
                    var dishAlreadyExistsInRestaurant = await _context.Dishes
                    .AnyAsync(d => d.DishId == dish.DishId && d.Restaurants.Any(r => request.RestaurantId == r.RestaurantId));

                    if (dishAlreadyExistsInRestaurant)
                    {
                        apiResponse.Success = false;
                        apiResponse.Errors.Add("Dish already exists in restaurant");

                        return apiResponse;
                    }
                }                

                dish = _mapper.Map<Dish>(request);
                dish.DishId = Guid.NewGuid();

                var restaurant = await _context.Restaurants
                    .FirstOrDefaultAsync(r => request.RestaurantId == r.RestaurantId);

                dish.Restaurants.Add(restaurant);

                await _context.Dishes.AddAsync(dish);
                await _context.SaveChangesAsync();

                var dishResponse = _mapper.Map<DishResponseDto>(dish);

                apiResponse.Success = true;
                apiResponse.Data = dishResponse;

                return apiResponse;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating dish");
                throw;
            }
        }
    }
}
