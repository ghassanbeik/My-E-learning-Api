

namespace Horizon.Domain.Events.LiveSessionEvents
{
    public class LiveSessionScheduledEvent : DomainEvent
    {
        public Guid SessionId { get; init; }
        public Guid CourseId { get; init; }
        public Guid InstructorId { get; init; }
        public string CourseTitle { get; init; } = string.Empty;
        public string SessionTitle { get; init; } = string.Empty;
        public DateTime ScheduledAt { get; init; }
    }
}
