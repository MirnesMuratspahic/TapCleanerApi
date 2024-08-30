using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TapCleaner.Models;
using TapCleaner.Models.DTO;
using TapCleaner.Services;
using TapCleaner.Services.Interfaces;

namespace TapCleaner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService containerService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ContainerController(IContainerService _containerService, IHttpContextAccessor _httpContextAccessor)
        {
            containerService = _containerService;
            httpContextAccessor = _httpContextAccessor;
        }

        [Authorize]
        [HttpGet("getcontainers")]
        public async Task<IActionResult> GetUsers()
        {
            var (errorStatus, containers) = await containerService.GetContainers();
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(containers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("addcontainer")]
        public async Task<IActionResult> AddContainer(Container container)
        {
            var errorStatus = await containerService.AddContainer(container);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus);
        }
    }
}
