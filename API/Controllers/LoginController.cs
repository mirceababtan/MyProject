using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginData data)
        {
            return Ok(data);
        }
    }

    public class LoginData
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
