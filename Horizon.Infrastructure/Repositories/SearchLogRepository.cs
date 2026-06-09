
using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class SearchLogRepository : Repository<SearchLog>, ISearchLogRepository
    {
        public SearchLogRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<string>> GetPopularQueriesAsync(int count, CancellationToken ct = default)
            => await _dbSet
                .GroupBy(sl => sl.Query)
                .OrderByDescending(g => g.Count())
                .Take(count)
                .Select(g => g.Key)
                .ToListAsync(ct);

        public async Task<IEnumerable<SearchLog>> GetByUserAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Where(sl => sl.UserId == userId)
                .OrderByDescending(sl => sl.SearchedAt)
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
