

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class CourseAnalyticsRepository : Repository<CourseAnalytics>, ICourseAnalyticsRepository
    {
        public CourseAnalyticsRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<CourseAnalytics>> GetByCourseAndDateRangeAsync(Guid courseId, DateTime from, DateTime to, CancellationToken ct = default)
            => await _dbSet
                .Where(ca => ca.CourseId == courseId && ca.Date >= from && ca.Date <= to)
                .OrderBy(ca => ca.Date)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<CourseAnalytics?> GetByCourseDateAsync(Guid courseId, DateTime date, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(ca => ca.CourseId == courseId && ca.Date == date.Date, ct);

        public async Task UpsertDailyAsync(CourseAnalytics analytics, CancellationToken ct = default)
        {
            var existing = await GetByCourseDateAsync(analytics.CourseId, analytics.Date, ct);
            if (existing == null)
                await _dbSet.AddAsync(analytics, ct);
            else
            {
                existing.NewEnrollments += analytics.NewEnrollments;
                existing.Completions += analytics.Completions;
                existing.Reviews += analytics.Reviews;
                existing.Revenue += analytics.Revenue;
                existing.Refunds += analytics.Refunds;
                existing.UniqueVisitors += analytics.UniqueVisitors;
                existing.VideoViews += analytics.VideoViews;
                existing.QuizAttempts += analytics.QuizAttempts;
                existing.AssignmentSubmissions += analytics.AssignmentSubmissions;
                existing.WishlistAdds += analytics.WishlistAdds;
                existing.CartAdds += analytics.CartAdds;
            }
        }
    }

}
