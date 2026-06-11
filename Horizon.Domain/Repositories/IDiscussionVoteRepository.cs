
using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IDiscussionVoteRepository : IRepository<DiscussionVote>
    {
        Task<DiscussionVote?> GetByUserAndReplyAsync( Guid UserId, Guid ReplyId, CancellationToken ct = default);
    }
}
