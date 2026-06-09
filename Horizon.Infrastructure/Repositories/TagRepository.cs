

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Tag?> GetByNameAsync(string name, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(t => t.Name == name, ct);

        public async Task<IEnumerable<Tag>> GetPopularTagsAsync(int count, CancellationToken ct = default)
            => await _dbSet.OrderByDescending(t => t.UsageCount).Take(count).AsNoTracking().ToListAsync(ct);

        public async Task<IEnumerable<Tag>> SearchTagsAsync(string query, CancellationToken ct = default)
            => await _dbSet.Where(t => t.Name.Contains(query)).AsNoTracking().ToListAsync(ct);

        public async Task IncrementUsageCountAsync(Guid tagId, CancellationToken ct = default)
        {
            var tag = await _dbSet.FindAsync(new object[] { tagId }, ct);
            if (tag != null) tag.UsageCount++;
        }
    }

}
