

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Horizon.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class DiscussionVoteRepository
     : Repository<DiscussionVote>, IDiscussionVoteRepository
    {
        public DiscussionVoteRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<DiscussionVote?> GetByUserAndReplyAsync(Guid UserId, Guid ReplyId, CancellationToken ct = default)
         => await _dbSet.FirstOrDefaultAsync(d => d.UserId == UserId && d.ReplyId == ReplyId, ct);
    }
}
