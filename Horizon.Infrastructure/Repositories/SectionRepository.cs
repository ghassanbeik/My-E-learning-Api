

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        public SectionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Section>> GetByCourseAsync(Guid courseId, CancellationToken ct = default)
            => await _dbSet
                .Where(s => s.CourseId == courseId)
                .OrderBy(s => s.DisplayOrder)
                .AsNoTracking()
                .ToListAsync(ct);

        public async Task<Section?> GetWithLessonsAsync(Guid sectionId, CancellationToken ct = default)
            => await _dbSet
                .Include(s => s.Lessons.OrderBy(l => l.DisplayOrder))
                .FirstOrDefaultAsync(s => s.Id == sectionId, ct);

        public async Task ReorderAsync(Guid courseId, IEnumerable<(Guid SectionId, int Order)> orders, CancellationToken ct = default)
        {
            var sections = await _dbSet.Where(s => s.CourseId == courseId).ToListAsync(ct);
            foreach (var (sectionId, order) in orders)
            {
                var section = sections.FirstOrDefault(s => s.Id == sectionId);
                if (section != null) section.DisplayOrder = order;
            }
        }
    }

}
