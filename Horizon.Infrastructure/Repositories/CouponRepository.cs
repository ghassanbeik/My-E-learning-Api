

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        public CouponRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Coupon?> GetByCodeAsync(string code, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(c => c.Code == code.ToUpper(), ct);

        public async Task<Coupon?> GetWithDetailsAsync(Guid couponId, CancellationToken ct = default)
            => await _dbSet
                .Include(c => c.ApplicableCourses).ThenInclude(cc => cc.Course)
                .Include(c => c.ApplicableCategories).ThenInclude(cc => cc.Category)
                .Include(c => c.Usages)
                .FirstOrDefaultAsync(c => c.Id == couponId, ct);

        public async Task<bool> IsValidAsync(string code, Guid? courseId = null, CancellationToken ct = default)
        {
            var coupon = await GetByCodeAsync(code, ct);
            if (coupon == null || !coupon.IsActive) return false;
            if (coupon.ExpiryDate.HasValue && coupon.ExpiryDate < DateTime.UtcNow) return false;
            if (coupon.MaxUses.HasValue && coupon.CurrentUses >= coupon.MaxUses) return false;
            if (courseId.HasValue && coupon.ApplicableCourses.Any() &&
                !coupon.ApplicableCourses.Any(cc => cc.CourseId == courseId)) return false;
            return true;
        }

        public async Task<bool> HasUserUsedAsync(string code, Guid userId, CancellationToken ct = default)
        {
            var coupon = await GetByCodeAsync(code, ct);
            if (coupon == null) return false;
            return await _context.Set<CouponUsage>()
                .AnyAsync(cu => cu.CouponId == coupon.Id && cu.UserId == userId, ct);
        }

        public async Task IncrementUsageAsync(Guid couponId, CancellationToken ct = default)
        {
            var coupon = await _dbSet.FindAsync(new object[] { couponId }, ct);
            if (coupon != null) coupon.CurrentUses++;
        }

        public async Task<IEnumerable<Coupon>> GetActiveAsync(CancellationToken ct = default)
            => await _dbSet
                .Where(c => c.IsActive &&
                            (c.ExpiryDate == null || c.ExpiryDate > DateTime.UtcNow) &&
                            (c.MaxUses == null || c.CurrentUses < c.MaxUses))
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
