

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IProgressRepository : IRepository<Progress>
    {
        Task<Progress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid lessonId, CancellationToken ct = default);
        Task<IEnumerable<Progress>> GetByEnrollmentAsync(Guid enrollmentId, CancellationToken ct = default);
        Task<int> GetCompletedCountAsync(Guid enrollmentId, CancellationToken ct = default);
        Task<double> GetCompletionPercentageAsync(Guid enrollmentId, CancellationToken ct = default);
        Task MarkCompleteAsync(Guid enrollmentId, Guid lessonId, CancellationToken ct = default);
    }
}
