using AuthImplementation.DTOs;
using AuthImplementation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthImplementation.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _authService.Register(userForRegistration);

            if(result.Succeeded)
            {
                return StatusCode(201);
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var userModel = await _authService.Login(userForAuthentication);
            if(userModel == null)
            {
                return BadRequest("Invalid email or password");
            }

            return Ok(userModel);
        }
    }
}
