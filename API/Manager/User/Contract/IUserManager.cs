namespace API.Manager.User.Contract
{
    public interface IUserManager
    {
        public User GetUserByEmail(string email);
    }
}
