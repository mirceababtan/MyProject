using API.Manager.User.Contract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Infrastructure.Utils
{
    public class GenerateToken
    {
        public static JwtSecurityToken GenerateAccessToken(User user,IConfiguration configuration)
        {

            var claims = new List<Claim>
        {
            new Claim("id",user.UserID.ToString()),
            new Claim("username", user.Username),
            new Claim("email", user.Email),
            new Claim("role", user.Role),
            new Claim("memberSince", user.CreatedAt.ToString()),
        };

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token expiration time
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
                    SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
