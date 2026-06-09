

namespace Horizon.Domain.Events.PaymentEvents
{
    public class PaymentFailedEvent : DomainEvent
    {
        public Guid UserId { get; init; }
        public Guid CourseId { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public string FailureReason { get; init; } = string.Empty;
    }
}
