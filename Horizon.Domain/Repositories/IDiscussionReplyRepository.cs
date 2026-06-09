
using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IDiscussionReplyRepository : IRepository<DiscussionReply>
    {
        Task<IEnumerable<DiscussionReply>> GetByDiscussionAsync(Guid discussionId, CancellationToken ct = default);
        Task<IEnumerable<DiscussionReply>> GetChildRepliesAsync(Guid parentReplyId, CancellationToken ct = default);
    }
}
