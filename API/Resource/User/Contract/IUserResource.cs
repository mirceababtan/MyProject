namespace API.Resource.User.Contract
{
    public interface IUserResource
    {
        public Task<User> SearchUserByUsernameAndEmailAsync(string username, string email);

        public Task<Contract.User> SearchUserByUsernameAndEmailAsync(string userData);

        public Task InsertUser(Contract.User user);
    }
}
