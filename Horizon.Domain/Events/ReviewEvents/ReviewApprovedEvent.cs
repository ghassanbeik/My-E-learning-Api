

namespace Horizon.Domain.Events.ReviewEvents
{
    public class ReviewApprovedEvent : DomainEvent
    {
        public Guid ReviewId { get; init; }
        public Guid CourseId { get; init; }
    }
}
