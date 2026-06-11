

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class CouponCourseRepository
    : Repository<CouponCourse>, ICouponCourseRepository
    {
        public CouponCourseRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
