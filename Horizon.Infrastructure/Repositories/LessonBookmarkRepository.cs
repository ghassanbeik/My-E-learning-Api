

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class LessonBookmarkRepository : Repository<LessonBookmark>, ILessonBookmarkRepository
    {
        public LessonBookmarkRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<LessonBookmark>> GetByUserAndCourseAsync(Guid userId, Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(b => b.Lesson).ThenInclude(l => l.Section)
                .Where(b => b.UserId == userId && b.Lesson.Section.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<bool> ExistsAsync(Guid userId, Guid lessonId, int? timestamp, CancellationToken ct = default)
            => await _dbSet.AnyAsync(b => b.UserId == userId &&
                                          b.LessonId == lessonId &&
                                          b.VideoTimestampSeconds == timestamp, ct);
    }

}
