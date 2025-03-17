using BLL.DTO;
using BLL.ServicesInterfaces;
using BLL.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserDTO> _userValidator;

        public UsersController(IUserService userService, IValidator<UserDTO> userValidator)
        {
            _userService = userService;
            _userValidator = userValidator;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("User is not authenticated");

            var user = await _userService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound("User not found");

            var validationResults = _userValidator.Validate(user);
            if (validationResults.Count > 0)
            {
                return BadRequest(validationResults);
            }

            return Ok(user);
        }

        [HttpGet("admin-data")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult GetAdminData()
        {
            return Ok("This endpoint is accessible only to Admins!");
        }
    }
}
