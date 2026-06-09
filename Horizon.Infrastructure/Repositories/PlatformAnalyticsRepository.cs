
using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class PlatformAnalyticsRepository : Repository<PlatformAnalytics>, IPlatformAnalyticsRepository
    {
        public PlatformAnalyticsRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<PlatformAnalytics>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default)
            => await _dbSet
                .Where(pa => pa.Date >= from && pa.Date <= to)
                .OrderBy(pa => pa.Date)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task UpsertDailyAsync(PlatformAnalytics analytics, CancellationToken ct = default)
        {
            var existing = await _dbSet.FirstOrDefaultAsync(pa => pa.Date == analytics.Date.Date, ct);
            if (existing == null)
                await _dbSet.AddAsync(analytics, ct);
            else
            {
                existing.NewUsers += analytics.NewUsers;
                existing.NewCourses += analytics.NewCourses;
                existing.TotalEnrollments += analytics.TotalEnrollments;
                existing.TotalRevenue += analytics.TotalRevenue;
                existing.ActiveUsers += analytics.ActiveUsers;
                existing.NewInstructors += analytics.NewInstructors;
                existing.CoursesPublished += analytics.CoursesPublished;
                existing.CertificatesIssued += analytics.CertificatesIssued;
            }
        }
    }

}
