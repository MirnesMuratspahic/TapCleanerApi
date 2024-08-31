using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TapCleaner.Models;
using TapCleaner.Models.DTO;
using TapCleaner.Services;
using TapCleaner.Services.Interfaces;

namespace TapCleaner.Controllers
{
    [Route("[controller]")]
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
        [HttpGet("GetContainers")]
        public async Task<IActionResult> GetUsers()
        {
            var (errorStatus, containers) = await containerService.GetContainers();
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(containers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddContainer")]
        public async Task<IActionResult> AddContainer(dtoContainer dtoContainer)
        {
            var errorStatus = await containerService.AddContainer(dtoContainer);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }

        [Authorize]
        [HttpPost("ReportContainer")]
        public async Task<IActionResult> ReportContainer(dtoReportContainer userContainer)
        {
            var errorStatus = await containerService.ReportContainer(userContainer);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CleanContainer")]
        public async Task<IActionResult> CleanContainer([FromBody] string name)
        {
            var errorStatus = await containerService.CleanContainer(name);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }
    }
}
