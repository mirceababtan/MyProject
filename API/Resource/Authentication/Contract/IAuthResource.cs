namespace API.Resource.Authentication.Contract
{
    public interface IAuthResource
    {
        public Task InsertRefreshToken(RefreshToken refreshToken);

        public Task DeleteRefreshToken(string refreshToken);

        public Task<RefreshToken?> GetRefreshToken(string refreshToken);
    }
}
