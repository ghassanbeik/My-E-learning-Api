

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class CouponCategoryRepository
    : Repository<CouponCategory>, ICouponCategoryRepository
    {
        public CouponCategoryRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
