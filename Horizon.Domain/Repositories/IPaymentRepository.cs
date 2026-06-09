
using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment?> GetByTransactionIdAsync(string transactionId, CancellationToken ct = default);
        Task<IEnumerable<Payment>> GetByUserAsync(Guid userId, CancellationToken ct = default);
        Task<IEnumerable<Payment>> GetByEnrollmentAsync(Guid enrollmentId, CancellationToken ct = default);
        Task<decimal> GetTotalRevenueAsync(DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
        Task<decimal> GetInstructorEarningsAsync(Guid instructorId, DateTime? from = null, DateTime? to = null, CancellationToken ct = default);
    }
}
