

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IRefundRequestRepository : IRepository<RefundRequest>
    {
        Task<IEnumerable<RefundRequest>> GetPendingAsync(CancellationToken ct = default);
        Task<IEnumerable<RefundRequest>> GetByUserAsync(Guid userId, CancellationToken ct = default);
        Task<RefundRequest?> GetByPaymentAsync(Guid paymentId, CancellationToken ct = default);
    }
}
