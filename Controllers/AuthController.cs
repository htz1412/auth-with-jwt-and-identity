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

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _authService.RegisterUser(userForRegistration);

            if(result.Succeeded)
            {
                return StatusCode(201);
            }

            return BadRequest(result.Errors);
        }
    }
}
