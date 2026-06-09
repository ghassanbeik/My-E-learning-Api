

using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class RefundRequest : AuditableEntity
    {
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public string Reason { get; set; } = string.Empty;
        public RefundStatus Status { get; set; } = RefundStatus.Pending;
        public DateTime? ResolvedAt { get; set; }
        public string? AdminNote { get; set; }
        public Guid? ResolvedById { get; set; }
        public UserInfo? ResolvedBy { get; set; }
    }
}
