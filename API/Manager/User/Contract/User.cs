namespace API.Manager.User.Contract
{
    public class User
    {
        public  Guid UserID { get; set; }
        public  string? Username { get; set; }
        public  string? Email { get; set; }
        public string Role { get; set; } = "Learner";
    }
}
