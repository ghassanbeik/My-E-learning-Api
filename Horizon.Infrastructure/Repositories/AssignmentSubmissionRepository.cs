
using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class AssignmentSubmissionRepository : Repository<AssignmentSubmission>, IAssignmentSubmissionRepository
    {
        public AssignmentSubmissionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<AssignmentSubmission?> GetByStudentAndAssignmentAsync(Guid studentId, Guid assignmentId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(s => s.StudentId == studentId && s.AssignmentId == assignmentId, ct);

        public async Task<IEnumerable<AssignmentSubmission>> GetPendingGradingAsync(Guid instructorId, CancellationToken ct = default)
            => await _dbSet
                .Include(s => s.Assignment).ThenInclude(a => a.Lesson).ThenInclude(l => l.Section).ThenInclude(s => s.Course)
                .Include(s => s.Student)
                .Where(s => !s.IsGraded &&
                            s.Assignment.Lesson.Section.Course.InstructorId == instructorId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<AssignmentSubmission>> GetByAssignmentAsync(Guid assignmentId, CancellationToken ct = default)
            => await _dbSet
                .Include(s => s.Student)
                .Where(s => s.AssignmentId == assignmentId)
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
