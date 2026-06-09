
using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class BundleRepository : Repository<Bundle>, IBundleRepository
    {
        public BundleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Bundle?> GetWithCoursesAsync(Guid bundleId, CancellationToken ct = default)
            => await _dbSet
                .Include(b => b.BundleCourses.OrderBy(bc => bc.DisplayOrder))
                    .ThenInclude(bc => bc.Course).ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(b => b.Id == bundleId, ct);

        public async Task<IEnumerable<Bundle>> GetActiveAsync(CancellationToken ct = default)
            => await _dbSet.Where(b => b.IsActive).AsNoTracking().ToListAsync(ct);
    }

}
