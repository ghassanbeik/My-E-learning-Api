

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class LiveSessionRepository : Repository<LiveSession>, ILiveSessionRepository
    {
        public LiveSessionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<LiveSession>> GetUpcomingAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Where(ls => ls.CourseId == courseId && ls.ScheduledAt > DateTime.UtcNow && !ls.IsCompleted)
                .OrderBy(ls => ls.ScheduledAt)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<LiveSession>> GetByInstructorAsync(Guid instructorId, CancellationToken ct = default)
            => await _dbSet
                .Where(ls => ls.InstructorId == instructorId)
                .OrderByDescending(ls => ls.ScheduledAt)
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
