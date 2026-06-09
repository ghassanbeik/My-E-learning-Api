

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class PayoutRepository : Repository<Payout>, IPayoutRepository
    {
        public PayoutRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Payout>> GetByInstructorAsync(Guid instructorId, CancellationToken ct = default)
            => await _dbSet
                .Where(p => p.InstructorId == instructorId)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Payout>> GetPendingAsync(CancellationToken ct = default)
            => await _dbSet
                .Include(p => p.Instructor)
                .Where(p => p.Status == PayoutStatus.Pending)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<decimal> GetTotalPaidAsync(Guid instructorId, CancellationToken ct = default)
            => await _dbSet
                .Where(p => p.InstructorId == instructorId && p.Status == PayoutStatus.Completed)
                .SumAsync(p => p.Amount, ct);
    }
}
