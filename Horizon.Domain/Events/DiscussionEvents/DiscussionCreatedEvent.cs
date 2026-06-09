

namespace Horizon.Domain.Events.DiscussionEvents
{
    public class DiscussionCreatedEvent : DomainEvent
    {
        public Guid DiscussionId { get; init; }
        public Guid CourseId { get; init; }
        public Guid InstructorId { get; init; }
        public string StudentName { get; init; } = string.Empty;
        public string CourseTitle { get; init; } = string.Empty;
        public string DiscussionTitle { get; init; } = string.Empty;
    }
}
