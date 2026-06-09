using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<Tag?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<IEnumerable<Tag>> GetPopularTagsAsync(int count, CancellationToken ct = default);
        Task<IEnumerable<Tag>> SearchTagsAsync(string query, CancellationToken ct = default);
        Task IncrementUsageCountAsync(Guid tagId, CancellationToken ct = default);
    }
}
