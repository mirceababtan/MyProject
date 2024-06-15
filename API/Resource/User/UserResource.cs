using API.Infrastructure.Database;
using API.Manager.User.Contract;
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
        public async Task<Contract.User?> SearchUserByUsernameAndEmailAsync(string username, string email)
        {
            IQueryable<Contract.User> query = _dbContext.Users;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Username == username || u.Email == email);
            }
            else if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(u => u.Username == username);
            }
            else if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email == email);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Contract.User?> GetUserById(Guid userId)
        {
            return await _dbContext.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        }


        public async Task InsertUser(Contract.User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Contract.User>> GetAllUsers()
        {
            return await _dbContext.Users.ToListAsync();
        }
    }
}
