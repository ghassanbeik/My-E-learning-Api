

namespace Horizon.Domain.Events.SubscriptionEvents
{
    public class InstructorSubscribedEvent : DomainEvent
    {
        public Guid InstructorId { get; init; }
        public Guid SubscriberId { get; init; }
        public string SubscriberName { get; init; } = string.Empty;
    }
}
