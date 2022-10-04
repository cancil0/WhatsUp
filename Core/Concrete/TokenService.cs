using Core.Abstract;
using Entity.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Concrete
{
    public class TokenService : ITokenService
    {
        private readonly string secretToken;
        public TokenService(IConfiguration configuration)
        {
            secretToken = configuration.GetValue<string>("JwtSettings:Secret");
        }
        public string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(secretToken);
            var secret = new SymmetricSecurityKey(key);
            SigningCredentials signingCredentials = new(secret, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("MobilePhone", user.MobilePhone),
                new Claim("UserName", user.UserName),
                new Claim("NameSurName", user.Name + user.Surname)
            };

            var tokenOptions = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public bool IsTokenValid(string token)
        {
            var mySecret = Encoding.UTF8.GetBytes(secretToken);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
