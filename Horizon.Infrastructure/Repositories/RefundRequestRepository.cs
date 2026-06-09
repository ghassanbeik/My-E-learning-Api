
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class RefundRequestRepository : Repository<RefundRequest>, IRefundRequestRepository
    {
        public RefundRequestRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<RefundRequest>> GetPendingAsync(CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.User)
                .Include(r => r.Payment)
                .Where(r => r.Status == RefundStatus.Pending)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<RefundRequest>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet.Where(r => r.UserId == userId).AsNoTracking().ToListAsync(ct);

        public async Task<RefundRequest?> GetByPaymentAsync(Guid paymentId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(r => r.PaymentId == paymentId, ct);
    }

}
