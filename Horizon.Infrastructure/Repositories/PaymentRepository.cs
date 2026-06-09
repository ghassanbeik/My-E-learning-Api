

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Payment?> GetByTransactionIdAsync(string transactionId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(p => p.TransactionId == transactionId, ct);

        public async Task<IEnumerable<Payment>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<Payment>> GetByEnrollmentAsync(Guid enrollmentId, CancellationToken ct = default)
            => await _dbSet.Where(p => p.EnrollmentId == enrollmentId).AsNoTracking().ToListAsync(ct);

        public async Task<decimal> GetTotalRevenueAsync(DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            var query = _dbSet.Where(p => p.Status == PaymentStatus.Completed);
            if (from.HasValue) query = query.Where(p => p.PaidAt >= from);
            if (to.HasValue) query = query.Where(p => p.PaidAt <= to);
            return await query.SumAsync(p => p.Amount, ct);
        }

        public async Task<decimal> GetInstructorEarningsAsync(Guid instructorId, DateTime? from = null, DateTime? to = null, CancellationToken ct = default)
        {
            var query = _dbSet.Where(p => p.Status == PaymentStatus.Completed &&
                                          p.Enrollment != null &&
                                          p.Enrollment.Course.InstructorId == instructorId);
            if (from.HasValue) query = query.Where(p => p.PaidAt >= from);
            if (to.HasValue) query = query.Where(p => p.PaidAt <= to);
            return await query.SumAsync(p => p.InstructorEarnings ?? 0, ct);
        }
    }
}
