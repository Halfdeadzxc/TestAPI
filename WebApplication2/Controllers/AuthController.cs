//using BLL.DTO;
//using BLL.Services;
//using BLL.ServicesInterfaces;
//using BLL.Validators;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace WebApplication2.Controllers
//{
//    [Route("api/auth")]
//    [ApiController]
//    public class AuthController : ControllerBase
//    {
//        private readonly IUserService _userService;
//        private readonly JwtService _jwtService;
//        private readonly IValidator<RefreshTokenDTO> _refreshTokenValidator;
//        private readonly IValidator<UserDTO> _userValidator;

//        public AuthController(
//            IUserService userService,
//            JwtService jwtService,
//            IValidator<RefreshTokenDTO> refreshTokenValidator,
//            IValidator<UserDTO> userValidator)
//        {
//            _userService = userService;
//            _jwtService = jwtService;
//            _refreshTokenValidator = refreshTokenValidator;
//            _userValidator = userValidator;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginModel model)
//        {
//            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
//            {
//                return BadRequest("Username and Password cannot be empty.");
//            }

//            var user = new UserDTO { Username = model.Username, Role = "User" }; 
//            var userValidationResults = _userValidator.Validate(user);
//            if (userValidationResults.Count > 0)
//            {
//                return BadRequest(userValidationResults);
//            }

//            var authenticatedUser = await _userService.AuthenticateUserAsync(model.Username, model.Password);
//            if (authenticatedUser == null)
//            {
//                return Unauthorized("Invalid username or password.");
//            }

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, authenticatedUser.Username),
//                new Claim(ClaimTypes.Role, authenticatedUser.Role)
//            };
//            var accessToken = _jwtService.GenerateAccessToken(claims, 15);

           
//            var refreshToken = await _userService.GenerateRefreshTokenAsync(authenticatedUser.Id);

//            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken.Token });
//        }

//        [HttpPost("refresh")]
//        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO model)
//        {
            
//            var refreshTokenValidationResults = _refreshTokenValidator.Validate(model);
//            if (refreshTokenValidationResults.Count > 0)
//            {
//                return BadRequest(refreshTokenValidationResults);
//            }

           
//            var isValid = await _userService.ValidateRefreshTokenAsync(model.Token);
//            if (!isValid)
//            {
//                return Unauthorized("Invalid or expired refresh token.");
//            }

           
//            var user = await _userService.GetUserByRefreshTokenAsync(model.Token);
//            if (user == null)
//            {
//                return Unauthorized("Invalid user.");
//            }

           
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.Username),
//                new Claim(ClaimTypes.Role, user.Role)
//            };
//            var accessToken = _jwtService.GenerateAccessToken(claims, 15);

//            return Ok(new { AccessToken = accessToken });
//        }
//    }

//    public class LoginModel
//    {
//        public string Username { get; set; }
//        public string Password { get; set; }
//    }
//}
