using TapCleaner.Models;
using TapCleaner.Models.DTO;
using TapCleaner.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TapCleaner.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserController(IUserService _userService, IHttpContextAccessor _httpContextAccessor)
        {
            userService = _userService;
            httpContextAccessor = _httpContextAccessor;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(dtoUserLogin dtoUser)
        {
            var (errorStatus, token) = await userService.Login(dtoUser);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var (errorStatus, users) = await userService.GetUsers();
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(users);
        }

        [Authorize]
        [HttpPut("UpdateUser/{email}")]
        public async Task<IActionResult> UpdateUser(string email, [FromBody] dtoUserUpdate user)
        {
            var errorStatus = await userService.UpdateUser(email,user);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }


        [Authorize]
        [HttpPost("GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail([FromBody] string email)
        {
            var (errorStatus, user) = await userService.GetUserByEmail(email);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("BlockUserByEmail{id}")]
        public async Task<IActionResult> BlockUserById([FromRoute] string email)
        {
            var errorStatus = await userService.BlockUserByEmail(email);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(dtoUserRegistration dtoUser)
        {
            var errorStatus = await userService.Register(dtoUser);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }

    }
}
