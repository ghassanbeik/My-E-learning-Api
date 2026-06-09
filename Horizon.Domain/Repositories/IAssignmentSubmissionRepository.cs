

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{

    public interface IAssignmentSubmissionRepository : IRepository<AssignmentSubmission>
    {
        Task<AssignmentSubmission?> GetByStudentAndAssignmentAsync(Guid studentId, Guid assignmentId, CancellationToken ct = default);
        Task<IEnumerable<AssignmentSubmission>> GetPendingGradingAsync(Guid instructorId, CancellationToken ct = default);
        Task<IEnumerable<AssignmentSubmission>> GetByAssignmentAsync(Guid assignmentId, CancellationToken ct = default);
    }
}
