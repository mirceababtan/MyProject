namespace API.Manager.User.Contract
{
    public class RegisterData
    {
        public required string Username { get; set; }
        public  required string Password { get; set; }
        public required string Email { get; set; }
    }
}
