

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Session?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(s => s.RefreshToken == refreshToken, ct);

        public async Task<IEnumerable<Session>> GetActiveSessionsAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Where(s => s.UserId == userId && s.RevokedAt == null && s.ExpiresAt > DateTime.UtcNow)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task RevokeAllUserSessionsAsync(Guid userId, CancellationToken ct = default)
        {
            var sessions = await _dbSet.Where(s => s.UserId == userId && s.RevokedAt == null).ToListAsync(ct);
            foreach (var session in sessions)
                session.RevokedAt = DateTime.UtcNow;
        }

        public async Task RevokeSessionAsync(string refreshToken, string ipAddress, CancellationToken ct = default)
        {
            var session = await GetByRefreshTokenAsync(refreshToken, ct);
            if (session == null) return;
            session.RevokedAt = DateTime.UtcNow;
            session.RevokedByIp = ipAddress;
        }
    }
}
