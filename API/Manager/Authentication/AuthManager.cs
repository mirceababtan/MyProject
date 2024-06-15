using API.Infrastructure.Utils;
using API.Manager.Authentication.Contract;
using API.Manager.User.Contract;
using API.Resource.Authentication.Contract;
using API.Resource.User.Contract;
using System.IdentityModel.Tokens.Jwt;

namespace API.Manager.Authentication
{
    public class AuthManager : IAuthManager
    {
        private readonly IAuthResource _authResource;
        private readonly IConfiguration _configuration;
        private readonly IUserManager _userManager;

        public AuthManager(IAuthResource authResource,IUserManager userManager,IConfiguration configuration)
        {
            _authResource = authResource;
            _configuration = configuration;
            _userManager = userManager;
        }

        private async Task InsertRefreshToken(string refreshToken,Guid userId)
        {
            var obj = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                UserId = userId,
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddHours(2),
                IsRevoked = false
            };
            await _authResource.InsertRefreshToken(obj);
        }

        private JwtSecurityToken GenerateAccessToken(User.Contract.User user)
        {
             return GenerateToken.GenerateAccessToken(user, _configuration); 
        }

        public async Task<object> ValidateUser(LoginData data)
        {
            var user = await _userManager.CheckUser(data);
            if (user.Username == data.User || user.Email == data.User)
            {
                var token = GenerateAccessToken(user);

                var refreshToken = Guid.NewGuid().ToString();
                await InsertRefreshToken(refreshToken,user.UserID);

                return (new {Message = "Login successful!" , Token = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = refreshToken });
            }

            return (new {Message = "Invalid credentials"});
        }



        public async Task<object> ValidateRefreshToken(RefreshRequest refreshRequest)
        {
            var refreshToken = await _authResource.GetRefreshToken(refreshRequest.RefreshToken);
            if(refreshToken != null && refreshToken.IsRevoked == false && DateTime.Compare(refreshToken.ExpiryDate,DateTime.Now) > 0)
            {
                var user = await _userManager.GetUserById(refreshRequest.UserId);
                var token = GenerateAccessToken(user);

                return (new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return (new { Message = "No valid refresh token."});
        }

        public async Task DeleteRefreshToken(string refreshToken)
        {
            await _authResource.DeleteRefreshToken(refreshToken);
        }
    }
}
