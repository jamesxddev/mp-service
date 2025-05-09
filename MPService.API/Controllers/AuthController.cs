using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPService.Application.Auth.DTOs;
using MPService.Application.Users;

namespace MPService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        public AuthController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _userAppService.LoginAsync(request);
                return Ok(result);

            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null)
            {   
                return BadRequest(new { message = "Registration request cannot be null." });
            }

            var result = await _userAppService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            return Ok(result.Value);
        }

        [Authorize]
        [HttpPut("password/{username}")]
        public async Task<IActionResult> UpdatePassword(string username, [FromBody] UpdatePasswordRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Update password request cannot be null." });
            }
            var result = await _userAppService.UpdatePasswordAsync(username, request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }
            return Ok(result);
        }
    }
}
