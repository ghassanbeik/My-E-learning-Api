

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class InstructorProfileRepository : Repository<InstructorProfile>, IInstructorProfileRepository
    {
        public InstructorProfileRepository(ApplicationDbContext context) : base(context) { }

        public async Task<InstructorProfile?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(ip => ip.UserId == userId, ct);

        public async Task<IEnumerable<InstructorProfile>> GetTopInstructorsAsync(int count, CancellationToken ct = default)
            => await _dbSet
                .Include(ip => ip.User)
                .OrderByDescending(ip => ip.AverageRating)
                .Take(count)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task UpdateEarningsAsync(Guid userId, decimal amount, CancellationToken ct = default)
        {
            var profile = await _dbSet.FirstOrDefaultAsync(ip => ip.UserId == userId, ct);
            if (profile == null) return;
            profile.TotalEarnings += amount;
            profile.PendingEarnings += amount;
        }
    }
}
