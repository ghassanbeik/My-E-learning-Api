

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Horizon.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class CourseCategoryRepository
    : Repository<CourseCategory>, ICourseCategoryRepository
    {
        public CourseCategoryRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<bool> ExistsAsync(Guid categoryId, CancellationToken ct = default)
          => await _dbSet.AnyAsync(c =>  c.CategoryId == categoryId, ct);
    }
}
