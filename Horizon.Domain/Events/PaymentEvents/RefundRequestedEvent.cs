

namespace Horizon.Domain.Events.PaymentEvents
{
    public class RefundRequestedEvent : DomainEvent
    {
        public Guid RefundRequestId { get; init; }
        public Guid PaymentId { get; init; }
        public Guid UserId { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
        public string Reason { get; init; } = string.Empty;
    }
}
