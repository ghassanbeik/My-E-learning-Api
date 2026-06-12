

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class ReviewResponseRepository
    : Repository<ReviewResponse>, IReviewResponseRepository
    {
        public ReviewResponseRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
