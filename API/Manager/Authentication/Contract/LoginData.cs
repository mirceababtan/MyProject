using System.ComponentModel.DataAnnotations;

namespace API.Manager.Authentication.Contract
{
    public class LoginData
    {
        [Required]
        public required string User { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
