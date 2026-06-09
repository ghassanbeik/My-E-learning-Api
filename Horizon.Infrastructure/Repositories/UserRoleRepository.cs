

using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Horizon.Infrastructure.Repositories
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> UserHasRoleAsync(Guid userId, string roleName, CancellationToken ct = default)
            => await _dbSet.AnyAsync(ur => ur.UserId == userId && ur.Role.Name == roleName, ct);

        public async Task<IEnumerable<string>> GetUserRoleNamesAsync(Guid userId, CancellationToken ct = default)
            => await _dbSet
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync(ct);

        public async Task<bool> AssignRoleAsync(Guid userId, Guid roleId, CancellationToken ct = default)
        {
            if (await _dbSet.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, ct))
                return false;
            await _dbSet.AddAsync(new UserRole { UserId = userId, RoleId = roleId }, ct);
            return true;
        }

        public async Task<bool> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken ct = default)
        {
            var userRole = await _dbSet.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, ct);
            if (userRole == null) return false;
            _dbSet.Remove(userRole);
            return true;
        }
    }
}
