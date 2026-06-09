

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class CourseCategoryRepository
    : Repository<CourseCategory>, ICourseCategoryRepository
    {
        public CourseCategoryRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
