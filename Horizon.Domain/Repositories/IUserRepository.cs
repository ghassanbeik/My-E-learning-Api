

using Horizon.Domain.Entities;

namespace Horizon.Domain.Repositories
{
    public interface IUserRepository : IRepository<UserInfo>
    {
        Task<UserInfo?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<UserInfo?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
        Task<UserInfo?> GetWithRolesAsync(Guid userId, CancellationToken ct = default);
        Task<UserInfo?> GetFullProfileAsync(Guid userId, CancellationToken ct = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
        Task<IEnumerable<UserInfo>> GetInstructorsAsync(CancellationToken ct = default);
        Task<IEnumerable<UserInfo>> SearchUsersAsync(string query, CancellationToken ct = default);
        Task<int> GetTotalStudentsCountAsync(CancellationToken ct = default);
    } 
}
