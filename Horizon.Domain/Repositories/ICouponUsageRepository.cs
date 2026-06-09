
using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ICouponUsageRepository : IRepository<CouponUsage>
    {
        Task<IEnumerable<CouponUsage>> GetByCouponAsync(Guid couponId, CancellationToken ct = default);
        Task<IEnumerable<CouponUsage>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
