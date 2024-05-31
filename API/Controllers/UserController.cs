using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Manager.User.Contract;

namespace API.Client.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("User")]
        public IActionResult GetUser() {
            return Ok(/*new User()
            {
                Id = Guid.NewGuid(),
                LastName = "Babtan",
                FirstName = "Mircea",
                Email = "mircea.babtan@gmail.com"
            }*/);
        }
    }
}
