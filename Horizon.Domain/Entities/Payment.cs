
using Horizon.Domain.Enums;

namespace Horizon.Domain.Entities
{
    public class Payment : AuditableEntity
    {
        public Guid? EnrollmentId { get; set; }
        public Enrollment? Enrollment { get; set; }
        public Guid UserId { get; set; }
        public UserInfo User { get; set; } = null!;
        public string TransactionId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? PaymentProviderTransactionId { get; set; }
        public decimal Amount { get; set; } = 0;
        public string Currency { get; set; } = "USD";
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? FailureReason { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? RefundedAt { get; set; }
        public decimal? RefundAmount { get; set; }
        public string? RefundReason { get; set; }
        public string? ReceiptUrl { get; set; }
        public string? BillingAddress { get; set; }
        public string? TaxDetails { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? PlatformFee { get; set; }
        public decimal? InstructorEarnings { get; set; }
        public ICollection<RefundRequest> RefundRequests { get; set; } = new List<RefundRequest>();
    }
}
