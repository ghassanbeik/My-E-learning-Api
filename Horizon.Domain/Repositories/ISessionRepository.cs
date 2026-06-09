

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ISessionRepository : IRepository<Session>
    {
        Task<Session?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
        Task<IEnumerable<Session>> GetActiveSessionsAsync(Guid userId, CancellationToken ct = default);
        Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken ct = default);
        Task RevokeSessionAsync(string refreshToken, string ipAddress, CancellationToken ct = default);
    }
}
