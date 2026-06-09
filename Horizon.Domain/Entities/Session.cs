

namespace Horizon.Domain.Entities
{
    public class Session : BaseEntity
    {
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public string RefreshToken { get; set; } = string.Empty;
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
        public bool IsActive => RevokedAt == null && ExpiresAt > DateTime.UtcNow;
    }

}
