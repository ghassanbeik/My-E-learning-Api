

using Horizon.Domain.Interfaces.Services.CurrentUserServices;
using Microsoft.AspNetCore.Http;

namespace Horizon.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext) => _httpContext = httpContext;

        public Guid? UserId
        {
            get
            {
                var claim = _httpContext.HttpContext?.User?.FindFirst("userId")?.Value;
                return Guid.TryParse(claim, out var id) ? id : null;
            }
        }

        public string? Email
            => _httpContext.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;

        public IEnumerable<string> Roles
            => _httpContext.HttpContext?.User?.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value) ?? Enumerable.Empty<string>();

        public bool IsAuthenticated
            => _httpContext.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public bool IsInRole(string role)
            => _httpContext.HttpContext?.User?.IsInRole(role) == true;
    }

}
