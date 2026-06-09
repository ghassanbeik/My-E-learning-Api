

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
        Task<bool> UserHasRoleAsync(Guid userId, string roleName, CancellationToken ct = default);
        Task<IEnumerable<string>> GetUserRoleNamesAsync(Guid userId, CancellationToken ct = default);
        Task<bool> AssignRoleAsync(Guid userId, Guid roleId, CancellationToken ct = default);
        Task<bool> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken ct = default);
    }
}
