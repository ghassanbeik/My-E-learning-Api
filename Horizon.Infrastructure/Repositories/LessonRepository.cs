

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Lesson>> GetBySectionAsync(Guid sectionId, CancellationToken ct = default)
            => await _dbSet
                .Where(l => l.SectionId == sectionId)
                .OrderBy(l => l.DisplayOrder)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<Lesson?> GetWithContentAsync(Guid lessonId, CancellationToken ct = default)
            => await _dbSet
                .Include(l => l.Quizzes).ThenInclude(q => q.Questions).ThenInclude(q => q.AnswerOptions)
                .Include(l => l.Assignments)
                .FirstOrDefaultAsync(l => l.Id == lessonId, ct);

        public async Task<IEnumerable<Lesson>> GetPreviewLessonsAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(l => l.Section)
                .Where(l => l.IsPreview && l.Section.CourseId == courseId)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task ReorderAsync(Guid sectionId, IEnumerable<(Guid LessonId, int Order)> orders, CancellationToken ct = default)
        {
            var lessons = await _dbSet.Where(l => l.SectionId == sectionId).ToListAsync(ct);
            foreach (var (lessonId, order) in orders)
            {
                var lesson = lessons.FirstOrDefault(l => l.Id == lessonId);
                if (lesson != null) lesson.DisplayOrder = order;
            }
        }

        public async Task<int> GetTotalDurationAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Include(l => l.Section)
                .Where(l => l.Section.CourseId == courseId)
                .SumAsync(l => l.DurationMinutes, ct);
    }

}
