using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Shared;

namespace Horizon.Domain.Repositories
{
    public interface IDiscussionRepository : IRepository<Discussion>
    {
        Task<Discussion?> GetWithRepliesAsync(Guid discussionId, CancellationToken ct = default);
        Task<PagedResult<Discussion>> GetByCourseAsync(Guid courseId, int page, int pageSize, DiscussionType? type = null, CancellationToken ct = default);
        Task<PagedResult<Discussion>> GetByLessonAsync(Guid lessonId, int page, int pageSize, CancellationToken ct = default);
        Task IncrementReplyCountAsync(Guid discussionId, CancellationToken ct = default);
        Task IncrementUpvoteAsync(Guid discussionId, CancellationToken ct = default);
        Task<IEnumerable<Discussion>> GetPinnedAsync(Guid courseId, CancellationToken ct = default);
    }
}
