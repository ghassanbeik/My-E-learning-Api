

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class DiscussionVoteRepository
     : Repository<DiscussionVote>, IDiscussionVoteRepository
    {
        public DiscussionVoteRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
