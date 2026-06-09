
using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        Task<Coupon?> GetByCodeAsync(string code, CancellationToken ct = default);
        Task<Coupon?> GetWithDetailsAsync(Guid couponId, CancellationToken ct = default);
        Task<bool> IsValidAsync(string code, Guid? courseId = null, CancellationToken ct = default);
        Task<bool> HasUserUsedAsync(string code, Guid userId, CancellationToken ct = default);
        Task IncrementUsageAsync(Guid couponId, CancellationToken ct = default);
        Task<IEnumerable<Coupon>> GetActiveAsync(CancellationToken ct = default);
    }

}
