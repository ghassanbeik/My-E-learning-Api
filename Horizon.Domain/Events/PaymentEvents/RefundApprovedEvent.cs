

namespace Horizon.Domain.Events.PaymentEvents
{
    public class RefundApprovedEvent : DomainEvent
    {
        public Guid RefundRequestId { get; init; }
        public Guid UserId { get; init; }
        public string UserEmail { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
        public decimal Amount { get; init; }
    }
}
