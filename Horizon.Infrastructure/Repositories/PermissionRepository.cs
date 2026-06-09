using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Horizon.Infrastructure.Repositories
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Permission>> GetByResourceAsync(string resource, CancellationToken ct = default)
            => await _dbSet.Where(p => p.Resource == resource).AsNoTracking().ToListAsync(ct);

        public async Task<IEnumerable<Permission>> GetByRoleAsync(Guid roleId, CancellationToken ct = default)
            => await _dbSet
                .Where(p => p.RolePermissions.Any(rp => rp.RoleId == roleId))
                .AsNoTracking()
                .ToListAsync(ct);
    }
}
