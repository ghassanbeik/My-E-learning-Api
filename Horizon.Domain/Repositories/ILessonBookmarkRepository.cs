

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ILessonBookmarkRepository : IRepository<LessonBookmark>
    {
        Task<IEnumerable<LessonBookmark>> GetByUserAndCourseAsync(Guid userId, Guid courseId, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid userId, Guid lessonId, int? timestamp, CancellationToken ct = default);
    }
}
