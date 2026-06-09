

using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class Payout : AuditableEntity
    {
        public Guid InstructorId { get; set; }
        public UserInfo Instructor { get; set; } = null!;
        public decimal Amount { get; set; } = 0;
        public string Currency { get; set; } = "USD";
        public PayoutStatus Status { get; set; } = PayoutStatus.Pending;
        public string PayoutMethod { get; set; } = string.Empty;
        public string? PayoutAccount { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? FailureReason { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int TotalEnrollments { get; set; } = 0;
        public decimal TotalRevenue { get; set; } = 0;
        public decimal PlatformFee { get; set; } = 0;
    }
}
