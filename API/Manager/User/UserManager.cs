using API.Manager.User.Contract;
using API.Resource.User.Contract;
using API.Infrastructure.Cryptography;

namespace API.Manager.User
{
    public class UserManager : IUserManager
    {
        private readonly IUserResource _userResource;

        public UserManager(IUserResource userResource)
        {
            _userResource = userResource;
        }

        public Contract.User GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertUser(RegisterData data)
        {
            var user = await _userResource.SearchUserByUsernameAndEmailAsync(data.Username,data.Email);

            if(user != null) return false;

            var (hashedPassword,salt) = PasswordHasher.HashPassword(data.Password);

            var userToInsert = new Resource.User.Contract.User(){
                Id = Guid.NewGuid(),
                Username = data.Username,
                Email = data.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                CreatedAt = DateTime.UtcNow,
            };

            await _userResource.InsertUser(userToInsert);
            return true;

        }

        public async Task<bool> CheckUser(LoginData loginData)
        {
            var user = await _userResource.SearchUserByUsernameAndEmailAsync(loginData.User);

            if(user == null) return false;
            //Add token generation and insertion in DB.

            return PasswordHasher.VerifyPassword(loginData.Password, user.PasswordHash, user.PasswordSalt);
        }
    }
}
