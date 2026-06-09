
using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class ProgressRepository : Repository<Progress>, IProgressRepository
    {
        public ProgressRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Progress?> GetByEnrollmentAndLessonAsync(Guid enrollmentId, Guid lessonId, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId && p.LessonId == lessonId, ct);

        public async Task<IEnumerable<Progress>> GetByEnrollmentAsync(Guid enrollmentId, CancellationToken ct = default)
            => await _dbSet.Where(p => p.EnrollmentId == enrollmentId).AsNoTracking().ToListAsync(ct);

        public async Task<int> GetCompletedCountAsync(Guid enrollmentId, CancellationToken ct = default)
            => await _dbSet.CountAsync(p => p.EnrollmentId == enrollmentId && p.IsCompleted, ct);

        public async Task<double> GetCompletionPercentageAsync(Guid enrollmentId, CancellationToken ct = default)
        {
            var total = await _dbSet.CountAsync(p => p.EnrollmentId == enrollmentId, ct);
            var completed = await _dbSet.CountAsync(p => p.EnrollmentId == enrollmentId && p.IsCompleted, ct);
            return total == 0 ? 0 : Math.Round((double)completed / total * 100, 2);
        }

        public async Task MarkCompleteAsync(Guid enrollmentId, Guid lessonId, CancellationToken ct = default)
        {
            var progress = await GetByEnrollmentAndLessonAsync(enrollmentId, lessonId, ct);
            if (progress == null)
            {
                await _dbSet.AddAsync(new Progress
                {
                    EnrollmentId = enrollmentId,
                    LessonId = lessonId,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow,
                }, ct);
            }
            else
            {
                progress.IsCompleted = true;
                progress.CompletedAt = DateTime.UtcNow;
            }
        }
    }
}
