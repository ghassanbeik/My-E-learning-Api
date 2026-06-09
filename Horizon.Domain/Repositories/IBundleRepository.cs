

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IBundleRepository : IRepository<Bundle>
    {
        Task<Bundle?> GetWithCoursesAsync(Guid bundleId, CancellationToken ct = default);
        Task<IEnumerable<Bundle>> GetActiveAsync(CancellationToken ct = default);
    }
}
