using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Horizon.Infrastructure.Repositories
{
    public class LessonNoteRepository : Repository<LessonNote>, ILessonNoteRepository
    {
        public LessonNoteRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<LessonNote>> GetByUserAndLessonAsync(Guid userId, Guid lessonId, CancellationToken ct = default)
            => await _dbSet
                .Where(n => n.UserId == userId && n.LessonId == lessonId)
                .OrderBy(n => n.VideoTimestampSeconds)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<IEnumerable<LessonNote>> GetByUserAndCourseAsync(Guid userId, Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(n => n.Lesson).ThenInclude(l => l.Section)
                .Where(n => n.UserId == userId && n.Lesson.Section.CourseId == courseId)
                .OrderBy(n => n.Lesson.Section.DisplayOrder)
                .ThenBy(n => n.Lesson.DisplayOrder)
                .ThenBy(n => n.VideoTimestampSeconds)
                .AsNoTracking()
                .ToListAsync(ct);
    }

}
