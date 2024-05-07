using Ipz_server.Models;
using Ipz_server.Models.Dto.Dishes;

namespace Ipz_server.Services.IServices
{
    public interface IDishService
    {
        public Task<ApiResponse> GetDishByIdAsync(Guid id);
        public Task<ApiResponse> AddDishToRestaurant(DishToRestaurantRequestDto request);
    }
}
