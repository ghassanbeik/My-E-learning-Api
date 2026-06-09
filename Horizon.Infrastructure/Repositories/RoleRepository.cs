using Horizon.Domain.Entities;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Horizon.Infrastructure.Repositories
{
    public class RoleRepository : Repository<RoleInfo>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context) { }

        public async Task<RoleInfo?> GetByNameAsync(string name, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(r => r.Name == name, ct);

        public async Task<IEnumerable<RoleInfo>> GetRolesWithPermissionsAsync(CancellationToken ct = default)
            => await _dbSet
                .Include(r => r.RolePermissions).ThenInclude(rp => rp.Permission)
                .AsNoTracking()
                .ToListAsync(ct);
    }

}
