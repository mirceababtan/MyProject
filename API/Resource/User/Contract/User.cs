namespace API.Resource.User.Contract
{
    public class User
    {
        public required Guid Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string PasswordSalt { get; set; }
        public required string Email { get; set; }
        public string Role { get; set; } = "Learner";
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
