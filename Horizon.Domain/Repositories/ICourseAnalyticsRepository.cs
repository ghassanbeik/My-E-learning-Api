

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ICourseAnalyticsRepository : IRepository<CourseAnalytics>
    {
        Task<IEnumerable<CourseAnalytics>> GetByCourseAndDateRangeAsync(Guid courseId, DateTime from, DateTime to, CancellationToken ct = default);
        Task<CourseAnalytics?> GetByCourseDateAsync(Guid courseId, DateTime date, CancellationToken ct = default);
        Task UpsertDailyAsync(CourseAnalytics analytics, CancellationToken ct = default);
    }
}
