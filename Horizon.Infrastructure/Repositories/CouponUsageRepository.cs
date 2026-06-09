

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class CouponUsageRepository : Repository<CouponUsage>, ICouponUsageRepository
    {
        public CouponUsageRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<CouponUsage>> GetByCouponAsync(Guid couponId, CancellationToken ct = default)
            => await _dbSet.Where(cu => cu.CouponId == couponId).AsNoTracking().ToListAsync(ct);

        public async Task<IEnumerable<CouponUsage>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet.Where(cu => cu.UserId == userId).AsNoTracking().ToListAsync(ct);
    }
}
