

using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Repositories;
using Horizon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Horizon.Infrastructure.Repositories;

// ─── User Repositories ───────────────────────────────────────────────────────

public class UserRepository : Repository<UserInfo>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }

    public async Task<UserInfo?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(u => u.Email == email.ToLower(), ct);

    public async Task<UserInfo?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);

    public async Task<UserInfo?> GetWithRolesAsync(Guid userId, CancellationToken ct = default)
        => await _dbSet
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

    public async Task<UserInfo?> GetFullProfileAsync(Guid userId, CancellationToken ct = default)
        => await _dbSet
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.InstructorProfiles)
            .Include(u => u.Enrollments).ThenInclude(e => e.Course)
            .Include(u => u.Certificates)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await _dbSet.AnyAsync(u => u.Email == email.ToLower(), ct);

    public async Task<IEnumerable<UserInfo>> GetInstructorsAsync(CancellationToken ct = default)
        => await _dbSet
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.InstructorProfiles)
            .Where(u => u.UserRoles.Any(ur => ur.Role.Name == "Instructor"))
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<IEnumerable<UserInfo>> SearchUsersAsync(string query, CancellationToken ct = default)
        => await _dbSet
            .Where(u => u.FirstName.Contains(query) ||
                        u.LastName.Contains(query) ||
                        u.Email.Contains(query))
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<int> GetTotalStudentsCountAsync(CancellationToken ct = default)
        => await _dbSet
            .CountAsync(u => u.UserRoles.Any(ur => ur.Role.Name == "Student"), ct);
}


