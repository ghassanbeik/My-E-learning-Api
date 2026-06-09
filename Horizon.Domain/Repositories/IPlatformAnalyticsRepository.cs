

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IPlatformAnalyticsRepository : IRepository<PlatformAnalytics>
    {
        Task<IEnumerable<PlatformAnalytics>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
        Task UpsertDailyAsync(PlatformAnalytics analytics, CancellationToken ct = default);
    }
}
