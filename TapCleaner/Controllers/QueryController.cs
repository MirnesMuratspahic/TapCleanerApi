using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TapCleaner.Models.DTO;
using TapCleaner.Services;
using TapCleaner.Services.Interfaces;

namespace TapCleaner.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IQueryService queryService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public QueryController(IQueryService _queryService, IHttpContextAccessor _httpContextAccessor)
        {
            queryService = _queryService;
            httpContextAccessor = _httpContextAccessor;
        }

        [HttpPost("AddQuery")]
        public async Task<IActionResult> AddQuery([FromBody] dtoUserQuery request)
        {
            var errorStatus = await queryService.AddQuery(request);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsersQueries")]
        public async Task<IActionResult> GetUsersQueries()
        {
            var (errorStatus, usersQueries) = await queryService.GetUsersQueries();
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(usersQueries);
        }
    }
}
