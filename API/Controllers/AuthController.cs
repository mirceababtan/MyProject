using API.Manager.Authentication.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginData data)
        {
           return Ok(await _authManager.ValidateUser(data));  
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            return Ok(await _authManager.ValidateRefreshToken(refreshRequest));
        }

        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout([FromQuery] string refreshToken)
        {
            await _authManager.DeleteRefreshToken(refreshToken);
            return Ok();
        }
    }
}
