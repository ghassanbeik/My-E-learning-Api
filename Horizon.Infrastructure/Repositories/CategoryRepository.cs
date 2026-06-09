using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Horizon.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync(CancellationToken ct = default)
            => await _dbSet.Where(c => c.ParentId == null).AsNoTracking().ToListAsync(ct);

        public async Task<IEnumerable<Category>> GetWithSubCategoriesAsync(CancellationToken ct = default)
            => await _dbSet
                .Where(c => c.ParentId == null)
                .Include(c => c.SubCategories)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<Category?> GetWithCoursesAsync(Guid categoryId, CancellationToken ct = default)
            => await _dbSet
                .Include(c => c.CourseCategories).ThenInclude(cc => cc.Course)
                .FirstOrDefaultAsync(c => c.Id == categoryId, ct);

        public async Task<IEnumerable<Category>> GetFeaturedAsync(CancellationToken ct = default)
            => await _dbSet
                .Where(c => c.IsFeatured)
                .OrderBy(c => c.DisplayOrder)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<bool> NameExistsAsync(string name, Guid? excludeId = null, CancellationToken ct = default)
            => await _dbSet.AnyAsync(c => c.Name == name && (!excludeId.HasValue || c.Id != excludeId), ct);
    }
}
