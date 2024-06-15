using System.ComponentModel.DataAnnotations;

namespace API.Manager.User.Contract
{
    public class RegisterData
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public  required string Password { get; set; }
        [Required]
        public required string Email { get; set; }
    }
}
