using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.DTO;
using BLL.ServicesInterfaces;
using System.Threading.Tasks;
using BLL.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;

namespace WebApplication2.Controllers
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest, CancellationToken cancellationToken)
        {
            var tokens = await _userService.AuthenticateAsync(loginRequest.Username, loginRequest.Password, cancellationToken);
            return Ok(tokens);
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest, CancellationToken cancellationToken)
        {
            var newAccessToken = await _userService.RefreshAccessTokenAsync(refreshRequest.RefreshToken, cancellationToken);
            return Ok(new { AccessToken = newAccessToken });
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDto, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateUserAsync(userDto, cancellationToken);
            return CreatedAtAction(nameof(Register), new { username = user.Username }, user);
        }

    }

}
