

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Repositories
{
    public class CourseTagRepository
     : Repository<CourseTag>, ICourseTagRepository
    {
        public CourseTagRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
