using Core.Extension;
using Entity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Core.Middleware
{
    public class Authentication : IMiddleware
    {
        private readonly IConfiguration configuration;
        public Authentication(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var isAllowAnonymousController = context.GetEndpoint().Metadata.GetMetadata<AllowAnonymousAttribute>();

            if (isAllowAnonymousController == null)
            {
                CheckToken(context);
            }

            await next(context);
        }

        private void CheckToken(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(token))
                {
                    throw new AppException("Core.Jwt.TokenNotFound", ExceptionTypes.NotFound.GetValue());
                }
                token = token.Split(" ").Last();
                context.Request.Headers.Authorization = string.Format("Bearer {0}", token);
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretToken = configuration.GetValue<string>("JwtSettings:Secret");
                var key = Encoding.ASCII.GetBytes(secretToken);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                UserInfoToken user = new()
                {
                    Email = jwtToken.Claims.First(x => x.Type == "email").Value,
                    UserName = jwtToken.Claims.First(x => x.Type == "userName").Value,
                    Id = jwtToken.Claims.First(x => x.Type == "id").Value,
                    Roles = jwtToken.Claims.Where(x => x.Type == "roles").Select(x => x.Value).ToList(),
                };

                context.Items.Add("UserName", user.UserName);

                if (user.Roles.Count == 0)
                {
                    throw new AppException("User.RoleNotFound", ExceptionTypes.NotFound.GetValue());
                }

                var controllerName = context.Request.RouteValues.First(x => x.Key == "controller").Value;
                var actionName = context.Request.RouteValues.First(x => x.Key == "action").Value;
                var routePath = string.Format("/api/{0}/{1}", controllerName, actionName);

            }
            catch (SecurityTokenExpiredException)
            {
                throw new AppException("Core.Jwt.ExpiredToken", ExceptionTypes.UnAuthorized.GetValue());
            }
            catch (SecurityTokenException)
            {
                throw new AppException("Core.Jwt.InvalidToken", ExceptionTypes.UnAuthorized.GetValue());
            }
            catch (AppException e)
            {
                throw new AppException(e.Message, e.ExceptionType);
            }
        }
    }

    public class UserInfoToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }

    }
}