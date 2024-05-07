using Ipz_server.Models.Dto.Dishes;
using Ipz_server.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ipz_server.Controllers
{
    [Route("api/dishes")]
    [ApiController]
    [Authorize]
    public class DishesController : ControllerBase
    {
        private readonly IDishService _dishService;
        public DishesController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDishToRestaurant([FromBody] DishToRestaurantRequestDto request)
        {
            try
            {
                var response = await _dishService.AddDishToRestaurant(request);
                Log.Information($"Dish create was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating dish");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
