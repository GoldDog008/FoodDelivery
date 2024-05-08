using Ipz;
using Ipz_server.Models.Dto.Dishes;
using Ipz_server.Models.Dto.Orders;
using Ipz_server.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ipz_server.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUserId = Guid.Parse(HttpContext.GetCurrentUserId());

            try
            {
                var response = await _orderService.GetAllOrders(currentUserId);
                Log.Information($"Orders was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while getting order");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequestDto request)
        {
            var currentUserId = Guid.Parse(HttpContext.GetCurrentUserId());

            try
            {
                var response = await _orderService.CreateOrder(currentUserId, request);
                Log.Information($"Order create was requested");

                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error while creating order");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
