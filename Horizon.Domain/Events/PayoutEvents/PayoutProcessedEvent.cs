

namespace Horizon.Domain.Events.PayoutEvents
{
    public class PayoutProcessedEvent : DomainEvent
    {
        public Guid PayoutId { get; init; }
        public Guid InstructorId { get; init; }
        public string InstructorEmail { get; init; } = string.Empty;
        public string InstructorName { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public string PeriodStart { get; init; } = string.Empty;
        public string PeriodEnd { get; init; } = string.Empty;
    }
}
