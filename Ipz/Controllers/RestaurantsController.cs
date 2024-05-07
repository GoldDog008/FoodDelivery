using Ipz_server.Models.Dto.Restaurants;
using Ipz_server.Services;
using Ipz_server.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ipz_server.Controllers
{
    [Route("api/restaurants")]
    [ApiController]
    [Authorize]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var response = await _restaurantService.GetRestaurantByIdAsync(id);
                Log.Information($"Restaurant with id {id} was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _restaurantService.GetAllRestaurants();
                Log.Information($"Restaurants was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] RestaurantCreateRequestDto request)
        {
            try
            {
                var response = await _restaurantService.CreateRestaurant(request);
                Log.Information($"Restaurant create was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating restaurant");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
