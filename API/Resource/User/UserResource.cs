using API.Infrastructure.Database;
using API.Resource.User.Contract;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace API.Resource.User
{
    public class UserResource : IUserResource
    {
        private readonly DatabaseContext _dbContext;
        public UserResource(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contract.User> SearchUserByUsernameAndEmailAsync(string userData)
        {
            var user = await _dbContext.Users.Where(u => u.Username == userData || u.Email == userData)
                                             .FirstOrDefaultAsync();
            return user;
        }

        public async Task<Contract.User> SearchUserByUsernameAndEmailAsync(string username, string email)
        {
            IQueryable<Contract.User> query = _dbContext.Users;

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.Username == username);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email == email);
            }

            var user = await query.FirstOrDefaultAsync();
            return user;
        }

        public async Task InsertUser(Contract.User user)
        {
            await _dbContext.Users.AddAsync(user);
            _dbContext.SaveChanges();
        }
    }
}
