namespace API.Manager.User.Contract
{
    public class User
    {
        public required Guid UserID { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string PasswordSalt {  get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; } = "Learner";
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
