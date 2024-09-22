using API.Manager.Authentication.Contract;

namespace API.Manager.User.Contract
{
    public interface IUserManager
    {
        public Task<User> GetUserById(Guid userId);

        public Task<User[]> GetAllUsers();

        public Task<bool> InsertUser(RegisterData data);

        public Task<User> CheckUser(LoginData login);

        public Task<bool> UpdateUser(Contract.UserUpdate user);
    }
}
