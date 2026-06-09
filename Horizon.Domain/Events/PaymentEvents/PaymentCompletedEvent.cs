

namespace Horizon.Domain.Events.PaymentEvents
{
    public class PaymentCompletedEvent : DomainEvent
    {
        public Guid PaymentId { get; init; }
        public Guid UserId { get; init; }
        public Guid CourseId { get; init; }
        public Guid InstructorId { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
        public string TransactionId { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public string Currency { get; init; } = "USD";
        public string PaymentMethod { get; init; } = string.Empty;
    }
}
