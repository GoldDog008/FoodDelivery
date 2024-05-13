using Ipz_server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Ipz_server.Controllers
{
    [Route("api/polling")]
    [ApiController]
    public class PollingController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCurrentTime()
        {
            Log.Information("Get current time");
            return Ok(new ApiResponse()
            {
                Success = true,
                Data = DateTime.Now
            });
        }
    }
}
