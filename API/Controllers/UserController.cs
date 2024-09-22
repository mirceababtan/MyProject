using API.Manager.User.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("All")]
        public async Task<User[]> GetAllUsers()
        {
            return await _userManager.GetAllUsers();
        }

        [AllowAnonymous]
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
                return Ok(response);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdate user)
        {
            bool result = await _userManager.UpdateUser(user);
            if (result)
            {
                var response = new
                {
                    message = "User successfully updated!",
                    updated = true
                };
                return Ok(response);
            }
            else
            {
                var response = new
                {
                    message = "User not found or update failed.",
                    updated = false
                };
                return NotFound(response);
            }
        }


    }
}
