

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IPayoutRepository : IRepository<Payout>
    {
        Task<IEnumerable<Payout>> GetByInstructorAsync(Guid instructorId, CancellationToken ct = default);
        Task<IEnumerable<Payout>> GetPendingAsync(CancellationToken ct = default);
        Task<decimal> GetTotalPaidAsync(Guid instructorId, CancellationToken ct = default);
    }
}
