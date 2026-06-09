

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IRoleRepository : IRepository<RoleInfo>
    {
        Task<RoleInfo?> GetByNameAsync(string name, CancellationToken ct = default);
        Task<IEnumerable<RoleInfo>> GetRolesWithPermissionsAsync(CancellationToken ct = default);
    }
}
