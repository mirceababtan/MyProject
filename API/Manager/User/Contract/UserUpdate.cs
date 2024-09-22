namespace API.Manager.User.Contract
{
    public class UserUpdate
    {
        public Guid Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }

    }
}
