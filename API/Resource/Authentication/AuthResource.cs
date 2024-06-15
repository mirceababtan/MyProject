using API.Infrastructure.Database;
using API.Resource.Authentication.Contract;
using Microsoft.EntityFrameworkCore;

namespace API.Resource.Authentication
{
    public class AuthResource : IAuthResource
    {
        private readonly DatabaseContext _dbContext;

        public AuthResource(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertRefreshToken(RefreshToken refreshToken)
        {
            await _dbContext.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteRefreshToken(string refreshToken)
        {
            var obj = await _dbContext.RefreshTokens.Where(r => r.Token == refreshToken).FirstOrDefaultAsync();
            if(obj != null)
            {
                _dbContext.RefreshTokens.Remove(obj);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
        {
            return await _dbContext.RefreshTokens.Where(r => r.Token == refreshToken).FirstOrDefaultAsync();
        }

    }
}
