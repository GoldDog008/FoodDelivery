using Ipz_server.Models.Dto.Restaurants;
using Ipz_server.Models;

namespace Ipz_server.Services.IServices
{
    public interface IRestaurantService
    {
        public Task<ApiResponse> GetRestaurantByIdAsync(Guid id);
        public Task<ApiResponse> CreateRestaurant(RestaurantCreateRequestDto request);
        public Task<ApiResponse> GetAllRestaurants();
    }
}
