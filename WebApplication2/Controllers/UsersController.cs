using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO;
using BLL.ServicesInterfaces;
using System.Threading.Tasks;
using BLL.Services;
using System.Security.Claims;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public UsersController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userService.AuthenticateUserAsync(loginDto.Username, loginDto.Password);

            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = _jwtService.GenerateAccessToken(claims, 60);

            return Ok(new { Token = token });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto)
        {
            var user = await _userService.CreateUserAsync(userDto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [Authorize(Policy = "AdminPolicy")] 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {username} not found" });
            }

            return Ok(user);
        }

        [Authorize(Policy = "UserPolicy")] 
        [HttpGet("ByUsername/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new { Message = $"User with username '{username}' not found" });
            }

            return Ok(user);
        }
    }
}
