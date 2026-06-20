using Horizon.Domain.Entities;
using System.Security.Claims;

namespace Horizon.Domain.Interfaces.Services.JWTServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(UserInfo user, IEnumerable<string> roles);

        /// <summary>
        /// Returns the UTC expiry a token generated right now would have,
        /// driven by Jwt:AccessTokenExpiryMinutes in configuration.
        /// Auth handlers must use this for the ExpiresAt field in responses
        /// instead of hardcoding AddHours(1).
        /// </summary>
        DateTime GetAccessTokenExpiry();

        string GenerateRefreshToken();
        Guid? ValidateAccessToken(string token);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
