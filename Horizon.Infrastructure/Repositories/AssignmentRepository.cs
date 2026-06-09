

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class AssignmentRepository : Repository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Assignment>> GetByLessonAsync(Guid lessonId, CancellationToken ct = default)
            => await _dbSet.Where(a => a.LessonId == lessonId).AsNoTracking().ToListAsync(ct);

        public async Task<Assignment?> GetWithSubmissionsAsync(Guid assignmentId, CancellationToken ct = default)
            => await _dbSet
                .Include(a => a.Submissions).ThenInclude(s => s.Student)
                .FirstOrDefaultAsync(a => a.Id == assignmentId, ct);
    }

}
