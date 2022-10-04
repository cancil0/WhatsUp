using Entity.Concrete;

namespace Core.Abstract
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        bool IsTokenValid(string token);
    }
}
