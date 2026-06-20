using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class VerificationTokenRepository
        : Repository<VerificationToken>, IVerificationTokenRepository
    {
        public VerificationTokenRepository(ApplicationDbContext context) : base(context) { }

        public async Task<VerificationToken?> GetValidTokenAsync(
            string tokenHash, VerificationTokenType type, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            return await _dbSet.FirstOrDefaultAsync(t =>
                t.TokenHash == tokenHash &&
                t.Type      == type      &&
                t.UsedAt    == null      &&
                t.ExpiresAt >  now, ct);
        }

        public async Task InvalidateActiveTokensAsync(
            Guid userId, VerificationTokenType type, CancellationToken ct = default)
        {
            var now    = DateTime.UtcNow;
            var active = await _dbSet
                .Where(t => t.UserId   == userId &&
                            t.Type     == type   &&
                            t.UsedAt   == null   &&
                            t.ExpiresAt > now)
                .ToListAsync(ct);

            foreach (var t in active)
                t.UsedAt = now;
        }
    }
}
