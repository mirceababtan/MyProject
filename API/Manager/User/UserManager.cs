using API.Manager.User.Contract;
using API.Resource.User.Contract;
using API.Infrastructure.Utils;
using API.Manager.Authentication.Contract;

namespace API.Manager.User
{
    public class UserManager : IUserManager
    {
        private readonly IUserResource _userResource;

        public UserManager(IUserResource userResource)
        {
            _userResource = userResource;
        }

        public async Task<Contract.User> GetUserById(Guid userId)
        {
            var user = await _userResource.GetUserById(userId);
            if (user == null) return new Contract.User();

            return new Contract.User()
            {
                UserID = user.Id,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
            };
        }

        public async Task<Contract.User[]> GetAllUsers()
        {
            var users = await _userResource.GetAllUsers();
            if(users == null || users.Count == 0) return Array.Empty<Contract.User>();

            return users.Select(u => new Contract.User()
            {
                UserID = u.Id,
                Email = u.Email,
                Username = u.Username,
                Role = u.Role,
            }).ToArray();
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

        public async Task<Contract.User> CheckUser(LoginData loginData)
        {
            var user = await _userResource.SearchUserByUsernameAndEmailAsync(loginData.User,loginData.User);

            if(user == null) return new();

            if(PasswordHasher.VerifyPassword(loginData.Password, user.PasswordHash, user.PasswordSalt))
            {
                return (new()
                {
                    Username = user.Username,
                    Email = user.Email,
                    UserID = user.Id,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                });
            }
            return new();
        }

        public async Task<bool> UpdateUser(Contract.UserUpdate user)
        {
            var existingUser = await _userResource.GetUserById(user.Id);
            if (existingUser == null) return false;

            // Update only if the field is provided
            if (!string.IsNullOrEmpty(user.Username))
            {
                existingUser.Username = user.Username;
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                existingUser.Email = user.Email;
            }
            if (!string.IsNullOrEmpty(user.Role))
            {
                existingUser.Role = user.Role;
            }
            // Password change (if provided)
            /*if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                var (hashedPassword, salt) = PasswordHasher.HashPassword(user.PasswordHash);
                existingUser.PasswordHash = hashedPassword;
                existingUser.PasswordSalt = salt;
            }*/

            return await _userResource.UpdateUser(existingUser);
        }

    }

}
