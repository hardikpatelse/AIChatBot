using AIChatBot.API.Data;
using AIChatBot.API.Interfaces.Services;
using AIChatBot.API.Models.Entities;
using AIChatBot.API.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIChatBot.API.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            var user = await _userService.RegisterUser(userRequest.Name, userRequest.Email);
            return Ok(user);
        }

        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User not found.");
        }
    }
}
