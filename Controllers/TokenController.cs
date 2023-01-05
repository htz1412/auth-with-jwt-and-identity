using AuthImplementation.DTOs;
using AuthImplementation.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthImplementation.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TokenController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var userDataModel = await _authService.RefreshToken(refreshTokenDto);
            if (userDataModel == null)
            {
                return BadRequest("Invalid token.");
            }

            return Ok(userDataModel);
        }
    }
}
