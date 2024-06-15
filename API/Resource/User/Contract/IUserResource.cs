namespace API.Resource.User.Contract
{
    public interface IUserResource
    {
        public Task<User> SearchUserByUsernameAndEmailAsync(string username, string email);

        public Task<User?> GetUserById(Guid userId);

        public Task InsertUser(Contract.User user);
        public Task<List<User>> GetAllUsers();
    }
}
