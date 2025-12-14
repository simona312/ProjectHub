using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectHub.Application.Dtos;
using ProjectHub.Application.InterFaces;

namespace ProjectHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            var userName = User.Identity?.Name;
            return Ok($"Welcome, {userName}! You are authorized ");
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            
            var success = await _userService.RegisterAsync(dto.UserName, dto.Email, dto.Password);
            if (!success)
                return BadRequest("User with that name or mail already exist.");
            return Ok("Registration is successful");


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var token = await _userService.LoginAsync(dto.UserName, dto.Password);
            if (token == null)
                return Unauthorized("Login failed");
            return Ok(new { Token = token });
        }


    }
}
