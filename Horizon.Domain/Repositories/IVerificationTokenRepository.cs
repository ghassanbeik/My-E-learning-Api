using Horizon.Domain.Entities;
using Horizon.Domain.Enums;

namespace Horizon.Domain.Repositories
{
    public interface IVerificationTokenRepository : IRepository<VerificationToken>
    {
        /// <summary>
        /// Finds a valid (unused, unexpired) token by its SHA-256 hash and type.
        /// Returns null when no matching token exists.
        /// </summary>
        Task<VerificationToken?> GetValidTokenAsync(
            string tokenHash, VerificationTokenType type, CancellationToken ct = default);

        /// <summary>
        /// Marks all currently-valid tokens of the given type for this user
        /// as used so only the latest issued token works.
        /// Call this before persisting a new token.
        /// </summary>
        Task InvalidateActiveTokensAsync(
            Guid userId, VerificationTokenType type, CancellationToken ct = default);
    }
}
