

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ILiveSessionRepository : IRepository<LiveSession>
    {
        Task<IEnumerable<LiveSession>> GetUpcomingAsync(Guid courseId, CancellationToken ct = default);
        Task<IEnumerable<LiveSession>> GetByInstructorAsync(Guid instructorId, CancellationToken ct = default);
    }
}
