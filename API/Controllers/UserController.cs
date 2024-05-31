using API.Manager.User.Contract;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("User")]
        public IActionResult GetUser([FromQuery] Guid userId)
        {
            return Ok(/*Check if user exists. Return if true*/);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterData data)
        {
            bool answer = await _userManager.InsertUser(data);
            if (answer == true)
            {
                var respone = new
                {
                    message = "User successfully created!",
                    created = true
                };
                return Ok(respone);
            }
            else
            {
                var response = new
                {
                    message = "This user already exists",
                    created = false
                };
                return BadRequest(response);
            }
        }

        public async Task<IActionResult> CheckUser([FromBody] LoginData data)
        {
            return Ok(await _userManager.CheckUser(data));
        }
    }
}
