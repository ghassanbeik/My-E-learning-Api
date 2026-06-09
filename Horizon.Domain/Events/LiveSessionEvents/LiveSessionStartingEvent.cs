

namespace Horizon.Domain.Events.LiveSessionEvents
{
    public class LiveSessionStartingEvent : DomainEvent
    {
        public Guid SessionId { get; init; }
        public Guid CourseId { get; init; }
        public string SessionTitle { get; init; } = string.Empty;
        public string MeetingUrl { get; init; } = string.Empty;
        public DateTime ScheduledAt { get; init; }
    }
}
