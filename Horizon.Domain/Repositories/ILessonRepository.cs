

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<IEnumerable<Lesson>> GetBySectionAsync(Guid sectionId, CancellationToken ct = default);
        Task<Lesson?> GetWithContentAsync(Guid lessonId, CancellationToken ct = default);
        Task<IEnumerable<Lesson>> GetPreviewLessonsAsync(Guid courseId, CancellationToken ct = default);
        Task ReorderAsync(Guid sectionId, IEnumerable<(Guid LessonId, int Order)> orders, CancellationToken ct = default);
        Task<int> GetTotalDurationAsync(Guid courseId, CancellationToken ct = default);
    }
}
