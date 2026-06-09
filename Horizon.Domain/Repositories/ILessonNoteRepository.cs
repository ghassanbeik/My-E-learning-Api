

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ILessonNoteRepository : IRepository<LessonNote>
    {
        Task<IEnumerable<LessonNote>> GetByUserAndLessonAsync(Guid userId, Guid lessonId, CancellationToken ct = default);
        Task<IEnumerable<LessonNote>> GetByUserAndCourseAsync(Guid userId, Guid courseId, CancellationToken ct = default);
    }
}
