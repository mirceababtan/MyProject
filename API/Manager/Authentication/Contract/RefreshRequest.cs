namespace API.Manager.Authentication.Contract
{
    public class RefreshRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
