using Horizon.Domain.Entities;
using System.Security.Claims;

namespace Horizon.Domain.Interfaces.Services.JWTServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserInfo user, IEnumerable<string> roles);
        string GenerateRefreshToken();
        Guid? ValidateAccessToken(string token);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
