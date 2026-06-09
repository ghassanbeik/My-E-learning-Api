using Horizon.Domain.Entities;


namespace Horizon.Domain.Repositories
{
    public interface ISearchLogRepository : IRepository<SearchLog>
    {
        Task<IEnumerable<string>> GetPopularQueriesAsync(int count, CancellationToken ct = default);
        Task<IEnumerable<SearchLog>> GetByUserAsync(Guid userId, CancellationToken ct = default);
    }
}
