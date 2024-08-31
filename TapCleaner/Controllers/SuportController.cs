using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TapCleaner.Models.DTO;
using TapCleaner.Services;
using TapCleaner.Services.Interfaces;

namespace TapCleaner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuportController : ControllerBase
    {
        readonly ISuportService suportService;

        public SuportController(ISuportService _suportService)
        {
            suportService = _suportService;
        }

        [HttpPost("GetClosestContainer")]
        public async Task<IActionResult> Login([FromBody] string coordinate)
        {
            var (errorStatus, message) = await suportService.GetClosestContainer(coordinate);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(message);
        }

    }
}
