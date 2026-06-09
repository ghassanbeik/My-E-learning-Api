

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<IEnumerable<Permission>> GetByResourceAsync(string resource, CancellationToken ct = default);
        Task<IEnumerable<Permission>> GetByRoleAsync(Guid roleId, CancellationToken ct = default);
    }
}
