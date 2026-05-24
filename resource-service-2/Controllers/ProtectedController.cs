using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace resource_service_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        [HttpGet("data")]
        [Authorize]
        public IActionResult GetProtectedData()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var username = User.Identity?.Name;

            return Ok(new
            {
                message = "This is protected data from **Resource Service 2**",
                service = "Resource Service 2",
                userId = userId,
                username = username,
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("public")]
        public IActionResult GetPublicData()
        {
            return Ok(new { 
                message = "This is public data from Resource Service 2 - no token required",
                service = "Resource Service 2"
            });
        }
    }
}
