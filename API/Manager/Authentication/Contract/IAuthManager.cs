namespace API.Manager.Authentication.Contract
{
    public interface IAuthManager
    {
        public Task<object> ValidateUser(LoginData data);

        public Task<object> ValidateRefreshToken(RefreshRequest refreshRequest);

        public Task DeleteRefreshToken(string refreshToken);
    }
}