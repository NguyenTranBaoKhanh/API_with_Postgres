using API_with_Postgres.Business.Interfaces;
using API_with_Postgres.Common.Response;
using API_with_Postgres.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_with_Postgres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Get-all")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommonResponse<dynamic>), 200)]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetAll();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(User user)
        {
            var createdUser = await _userService.CreateUser(user);
            return Ok(createdUser);
        }

        // Existing code...

        [HttpGet("by-email/{email}")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            var response = await _userService.GetUsersByEmail(email);
            if (response == null || !response.Payload.Any())
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
