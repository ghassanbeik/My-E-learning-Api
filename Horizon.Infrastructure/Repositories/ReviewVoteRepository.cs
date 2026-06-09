

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class ReviewVoteRepository
     : Repository<ReviewVote>, IReviewVoteRepository
    {
        public ReviewVoteRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
