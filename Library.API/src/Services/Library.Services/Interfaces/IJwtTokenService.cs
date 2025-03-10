using System.Security.Claims;

namespace Library.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(List<Claim> claims, string email, string username);
    }
}
