namespace API.Manager.User.Contract
{
    public interface IUserManager
    {
        public User GetUserByEmail(string email);

        public Task<bool> InsertUser(RegisterData data);

        public Task<bool> CheckUser(LoginData login);
    }
}
