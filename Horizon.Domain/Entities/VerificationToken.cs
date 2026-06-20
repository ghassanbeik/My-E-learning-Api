using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    /// <summary>
    /// Single-use, time-limited token for email-verification and
    /// password-reset flows. Only the SHA-256 hash of the raw token
    /// is stored — the raw value is only ever sent to the user's email.
    /// </summary>
    public class VerificationToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;

        public VerificationTokenType Type { get; set; }

        /// <summary>SHA-256 hash (lower-case hex) of the raw token emailed to the user.</summary>
        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        /// <summary>Set when the token has been consumed — prevents replay.</summary>
        public DateTime? UsedAt { get; set; }

        public bool IsValid => UsedAt == null && ExpiresAt > DateTime.UtcNow;
    }
}
