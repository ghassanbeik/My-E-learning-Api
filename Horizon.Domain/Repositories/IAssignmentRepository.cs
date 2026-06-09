

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<IEnumerable<Assignment>> GetByLessonAsync(Guid lessonId, CancellationToken ct = default);
        Task<Assignment?> GetWithSubmissionsAsync(Guid assignmentId, CancellationToken ct = default);
    }
}
