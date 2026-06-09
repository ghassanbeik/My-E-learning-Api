

namespace Horizon.Domain.Events.CourseEvents
{
    public class CourseRejectedEvent : DomainEvent
    {
        public Guid CourseId { get; init; }
        public Guid InstructorId { get; init; }
        public string CourseTitle { get; init; } = string.Empty;
        public string InstructorEmail { get; init; } = string.Empty;
        public string InstructorName { get; init; } = string.Empty;
        public string Reason { get; init; } = string.Empty;
    }
}
